using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Models;
using MPE.SS.Models.Graphs;

namespace MPE.SS.Interfaces
{
    internal interface IGraphRenderService
    {
        Graph GenerateGraph(Configuration configuration);
    }
}
