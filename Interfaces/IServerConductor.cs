using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Models;

namespace MPE.SS.Interfaces
{
    internal interface IServerConductor
    {
        Task ExecuteTests(Server server, ReportItem context);

        Task ExecuteDataCollection(Server server);
    }
}
