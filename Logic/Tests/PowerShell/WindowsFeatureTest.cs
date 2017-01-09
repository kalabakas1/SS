using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Interfaces;
using MPE.SS.Logic.Tests.Base;
using MPE.SS.Models;

namespace MPE.SS.Logic.Tests.PowerShell
{
    internal class WindowsFeatureTest : PowerShellTestAbstract<WindowsFeatureTest, string>
    {
        public WindowsFeatureTest(ITestCure<WindowsFeatureTest, string> cure)
            : base(cure)
        {
        }

        protected override string ScriptTempate
        {
            get { return "Get-WindowsFeature -Name '{0}'"; }
        }

        protected override Func<PSObject, string> Converter
        {
            get
            {
                return obj => obj.Properties["Installed"].Value.ToString();
            }
        }

        protected override List<string> GetTestInput(Server server)
        {
            return server.Features != null ? server.Features.ToList() : new List<string>(); ;
        }
        
        public override string Name
        {
            get { return "Windowsfeatures"; }
        }

        protected override bool EvaluateResult(string result)
        {
            return result == "True";
        }
    }
}
