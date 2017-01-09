using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Enums;
using MPE.SS.Interfaces;
using MPE.SS.Logic.Configurations;
using MPE.SS.Logic.Tests.PowerShell;
using MPE.SS.Models;

namespace MPE.SS.Logic.TestCures
{
    internal class WindowsFeatureInstallCure : PowerShellStringInputCureAbstract<WindowsFeatureTest>
    {
        public override string CommandTemplate
        {
            get { return "Install-WindowsFeature  -Name '{0}'"; }
        }

        public override string FailedMessage
        {
            get { return "Not able to fix issue - tried to install windows feature, but failed..."; }
        }

        public override string SuccessMessage
        {
            get { return "Issue fixed - installed windows feature..."; }
        }
    }
}
