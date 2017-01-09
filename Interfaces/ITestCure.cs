using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Models;

namespace MPE.SS.Interfaces
{
    internal interface ITestCure<T, TK>
        where T : ITest
    {
        ReportItem CureIssue(Server server, TK input, ReportItem context);
    }
}
