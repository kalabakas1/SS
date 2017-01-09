using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Enums;
using MPE.SS.Interfaces;
using MPE.SS.Logic.Configurations;
using MPE.SS.Models;

namespace MPE.SS.Logic.TestCures
{
    internal abstract class PowerShellStringInputCureAbstract<T> : ITestCure<T, string>
        where T : ITest
    {
        private PowerShellService _shellService;

        public abstract string CommandTemplate { get; }
        public abstract string FailedMessage { get; }
        public abstract string SuccessMessage { get; }

        public ReportItem CureIssue(Server server, string input, ReportItem context)
        {
            _shellService = new PowerShellService(server.Name, server.Credential.Username,
                server.Credential.Password);

            _shellService.Invoke(shell =>
            {
                shell.AddScript(string.Format(CommandTemplate, input));

                var result = shell.Invoke();
                if (shell.HadErrors)
                {
                    context.State = ReportItemState.Failure;
                    context.Header = FailedMessage;
                    context.Elaboration = string.Join("\n", _shellService.GetErrorsFromShell(shell));
                }
                else
                {
                    context.State = ReportItemState.Warning;
                    context.Header = SuccessMessage;
                }

            }, AppConfiguration.Configuration.AppState == AppState.Release);

            return context;
        }
    }
}
