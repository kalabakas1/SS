using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Enums;
using MPE.SS.Models.Graphs;

namespace MPE.SS.Interfaces
{
    internal interface IChartDataGenerator
    {
        string ChartName { get; }
        bool CanHandle(DataCollector type);

        Chart GenerateChart(string serverName);
    }
}
