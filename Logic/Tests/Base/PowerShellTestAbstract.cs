using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using MPE.SS.Enums;
using MPE.SS.Interfaces;
using MPE.SS.Logic.Configurations;
using MPE.SS.Models;

namespace MPE.SS.Logic.Tests.Base
{
    internal abstract class PowerShellTestAbstract<TK, T> : IServerTest
        where TK :ITest
    {
        protected string MessageTemplate
        {
            get { return "{0}"; }
        }
        protected abstract string ScriptTempate { get; }
        protected virtual List<string> ScriptFiles { get { return null; } }
        protected abstract Func<PSObject, string> Converter { get; }
        protected abstract List<T> GetTestInput(Server server);

        protected virtual Func<T, string> ConvertToExecutableScript
        {
            get { return x => string.Format(ScriptTempate, x); }
        }

        public abstract string Name { get; }

        private ITestCure<TK, T> _cure;

        public PowerShellTestAbstract(
            ITestCure<TK, T> cure)
        {
            _cure = cure;
        }


        public ReportItem Run(Server server, ReportItem context)
        {
            context.Header = Name;

            if (GetTestInput(server) == null || !GetTestInput(server).Any())
            {
                context.Delete = true;
                return context;
            }

            var shellService = new PowerShellService(server.Name, server.Credential.Username, server.Credential.Password);
            shellService.Invoke(shell =>
            {
                foreach (var input in GetTestInput(server))
                {
                    try
                    {
                        var executableScript = ConvertToExecutableScript.Invoke(input);
                        shellService.LoadFilesIntoShell(shell, ScriptFiles);
                        if (!string.IsNullOrEmpty(executableScript))
                            shell.AddScript(executableScript);
                        var results = shell.Invoke().ToList();

                        var convertedResult = shellService.ConvertResult(results, obj => Converter.Invoke(obj));
                        var state = convertedResult.FirstOrDefault();
                        var message = context.Create(string.Format(MessageTemplate, input, state));
                        message.State = EvaluateResult(state) ? ReportItemState.Success : ReportItemState.Failure;
                        if (shell.HadErrors)
                        {
                            message.Elaboration = string.Join("\n", shellService.GetErrorsFromShell(shell));
                        }

                        if (message.State == ReportItemState.Failure && AppConfiguration.Configuration.CureTestIssues &&
                            _cure != null)
                        {
                            var cureContext = message.Create();
                            _cure.CureIssue(server, input, cureContext);
                            cureContext.Update();
                        }
                        message.Update();
                    }
                    catch(Exception e)
                    {
                        var message = context.Create(string.Format(MessageTemplate, input, ""));
                        message.Elaboration = e.Message;
                        message.State = ReportItemState.Failure;
                        message.Update();
                    }
                }
                context.Update();

            }, AppConfiguration.Configuration.AppState == AppState.Release);
            
            return context;
        }

        protected abstract bool EvaluateResult(string result);
    }
}
