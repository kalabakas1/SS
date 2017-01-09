using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using MPE.SS.Interfaces;
using MPE.SS.Logic.Tests.Base;
using MPE.SS.Models;

namespace MPE.SS.Logic.Tests.PowerShell
{
    internal class ScheduledTaskAlive : PowerShellTestAbstract<ScheduledTaskAlive, string>
    {
        public ScheduledTaskAlive(ITestCure<ScheduledTaskAlive, string> cure)
            : base(cure)
        {
        }

        protected override string ScriptTempate
        {
            get { return "Get-ScheduledTask -TaskName {0}"; }
        }

        protected override Func<PSObject, string> Converter
        {
            get { return obj => obj.Members["State"].Value.ToString() == "1" ? "Disabled" : "Enabled"; }
        }

        protected override List<string> GetTestInput(Server server)
        {
            return server.ScheduledTasks != null ? server.ScheduledTasks.ToList() : new List<string>(); ;
        }

        public override string Name
        {
            get { return "Scheduledtasks"; }
        }

        protected override bool EvaluateResult(string result)
        {
            return result != "Disabled";
        }
    }
}
