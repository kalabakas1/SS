using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using MPE.SS.Interfaces;
using MPE.SS.Logic.Tests.Base;
using MPE.SS.Models;

namespace MPE.SS.Logic.Tests.PowerShell
{
    internal class AppPoolAlive : PowerShellTestAbstract<AppPoolAlive, string>
    {
        public AppPoolAlive(ITestCure<AppPoolAlive, string> cure)
            : base(cure)
        {
        }

        protected override string ScriptTempate { get { return "Get-WebAppPoolState -Name {0}"; } }

        protected override Func<PSObject, string> Converter
        {
            get
            {
                return obj => obj.Properties["Value"].Value.ToString();
            }
        }

        protected override List<string> GetTestInput(Server server)
        {
            return server.AppPools != null ? server.AppPools.ToList() : new List<string>(); ;
        }
        
        public override string Name
        {
            get { return "App-Pools"; }
        }

        protected override bool EvaluateResult(string result)
        {
            return result == "Started";
        }
    }
}
