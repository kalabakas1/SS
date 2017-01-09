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
    internal class ServerCpuChart : ChartAbstractBase<ServerUtilization>
    {
        private static object _lock = new object();

        public ServerCpuChart(
            IRepository<ServerUtilization> repository,
            IBuilder<Chart> chartBuilder,
            IBuilder<DataSet> dataSetBuilder,
            IBuilder<Point> point,
            IBaselineService baselineService)
            : base(chartBuilder, dataSetBuilder, point, repository, baselineService)
        {
        }

        protected override Chart DefineChart(List<ServerUtilization> data, string serverName)
        {
            lock (_lock)
            {
                _chartBuilder.Where(x => x.Server = serverName)
                    .Where(x => x.Name = ChartName)
                    .Where(x => x.DataCollector = DataCollector.SystemInfo);

                var dataSet = CreateDataSet(data, DatasetNames.CpuUtil, new Color(), z => z.CpuUsage ?? 0);
                UpdateBaseline(ChartAlias.ServerCpu, serverName, dataSet);

                _chartBuilder.Where(x => x.DataSets = new List<DataSet>
                {
                    dataSet,
                    CreateBaseline(dataSet, ChartAlias.ServerCpu, serverName, DatasetNames.CpuUtil)
                });
                return _chartBuilder.Build();
            }
        }

        public override bool CanHandle(DataCollector type)
        {
            return type == DataCollector.SystemInfo;
        }

        public override string ChartName { get { return "CPU % utilized on server"; } }
    }
}
