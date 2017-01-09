using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Enums;
using MPE.SS.Interfaces;
using MPE.SS.Models;

namespace MPE.SS.Logic.TestCures
{
    internal class NullCure<T,TK> : ITestCure<T,TK>
        where T : ITest
    {
        public ReportItem CureIssue(Server server, TK input, ReportItem context)
        {
            context.State = ReportItemState.Failure;
            context.Header = string.Format("No cure for {0} implemented - issue not fixed...", typeof(T).Name);
            return context;
        }
    }
}
