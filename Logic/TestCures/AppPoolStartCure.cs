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
    internal class AppPoolStartCure : PowerShellStringInputCureAbstract<AppPoolAlive>
    {
        public override string CommandTemplate
        {
            get { return "Start-WebAppPool -Name '{0}'"; }
        }

        public override string FailedMessage
        {
            get { return "Not able to fix issue - tried to start app-pool again, but failed..."; }
        }

        public override string SuccessMessage
        {
            get { return "Issue fixed - app-pool started again..."; }
        }
    }
}
