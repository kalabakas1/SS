using System.Collections.Generic;
using System.Linq;
using MPE.SS.Constants;
using MPE.SS.Enums;
using MPE.SS.Interfaces;
using MPE.SS.Logic.Configurations;
using MPE.SS.Models.Graphs;
using MPE.SS.Models.ServerInfo;

namespace MPE.SS.Logic.Charts.Servers
{
    internal class ServerMemoryChart : ChartAbstractBase<ServerUtilization>
    {
        private static object _lock = new object();

        public ServerMemoryChart(
            IRepository<ServerUtilization> repository,
            IBuilder<Chart> chartBuilder,
            IBuilder<DataSet> dataSetBuilder,
            IBuilder<Point> point,
            IBaselineService baselineService)
            : base(chartBuilder, dataSetBuilder, point, repository, baselineService)
        {
        }

        public override bool CanHandle(DataCollector type)
        {
            return type == DataCollector.SystemInfo;
        }

        protected override Chart DefineChart(List<ServerUtilization> data, string serverName)
        {
            lock (_lock)
            {
                _chartBuilder.Where(x => x.Server = serverName)
                   .Where(x => x.Name = ChartName)
                   .Where(x => x.DataCollector = DataCollector.SystemInfo);

                var dataSet = CreateDataSet(data, DatasetNames.MemoryUtil, new Color(), z => (int)(z.RamUsage / z.RamTotal * 100));
                UpdateBaseline(ChartAlias.ServerMemory, serverName, dataSet);

                _chartBuilder.Where(x => x.DataSets = new List<DataSet>
                {
                    dataSet,
                    CreateBaseline(dataSet, ChartAlias.ServerMemory, serverName, DatasetNames.MemoryUtil)
                }.Where(z => z != null).ToList());

                return _chartBuilder.Build();
            }
        }

        public override string ChartName { get { return "Memory % utilized on server"; } }
    }
}
