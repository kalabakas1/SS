using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Models;

namespace MPE.SS.Interfaces
{
    internal interface IConductor
    {
        ReportItem ExecuteTests(Configuration configuration);

        List<Task> ExecuteDataCollection(Configuration configuration);
    }
}
