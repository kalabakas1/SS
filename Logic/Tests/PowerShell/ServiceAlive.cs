using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using MPE.SS.Interfaces;
using MPE.SS.Logic.Tests.Base;
using MPE.SS.Models;

namespace MPE.SS.Logic.Tests.PowerShell
{
    internal class ServiceAlive : PowerShellTestAbstract<ServiceAlive, string>
    {
        public ServiceAlive(ITestCure<ServiceAlive, string> cure)
            : base(cure)
        {
        }

        protected override string ScriptTempate
        {
            get { return "Get-Service -Name '{0}'"; }
        }

        protected override Func<PSObject, string> Converter
        {
            get { return obj => obj.Members["Status"].Value.ToString(); }
        }

        protected override List<string> GetTestInput(Server server)
        {
            return server.Services != null ? server.Services.ToList() : new List<string>(); ;
        }

        public override string Name
        {
            get { return "Services"; }
        }

        protected override bool EvaluateResult(string result)
        {
            return result == "Running";
        }
    }
}
