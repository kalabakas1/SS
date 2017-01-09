using System.Collections.Generic;
using MPE.SS.Constants;
using MPE.SS.Enums;
using MPE.SS.Interfaces;
using MPE.SS.Models.Graphs;
using MPE.SS.Models.RedisInfo;

namespace MPE.SS.Logic.Charts.Redis
{
    internal class RedisCommandChart : ChartAbstractBase<RedisInfo>
    {
        private static object _lock = new object();

        public RedisCommandChart(
            IBuilder<Chart> chartBuilder,
            IBuilder<DataSet> dataSetBuilder,
            IBuilder<Point> point,
            IRepository<RedisInfo> repository,
            IBaselineService baselineService)
            : base(chartBuilder, dataSetBuilder, point, repository, baselineService)
        {
        }

        protected override Chart DefineChart(List<RedisInfo> data, string serverName)
        {
            lock (_lock)
            {
                _chartBuilder.Where(x => x.Server = serverName)
                    .Where(x => x.Name = ChartName)
                    .Where(x => x.DataCollector = DataCollector.RedisInfo);

                var dataset = PeakChart(CreateDataSet(data, DatasetNames.Commands, new Color(), z => (int)z.Statistic.TotalCommandProcessed));
                UpdateBaseline(ChartAlias.RedisCommands, serverName, dataset);
                _chartBuilder.Where(x => x.DataSets = new List<DataSet>
                {
                    dataset,
                    CreateBaseline(dataset, ChartAlias.RedisCommands, serverName, DatasetNames.Commands)
                });

                return _chartBuilder.Build();
            }
        }

        public override bool CanHandle(DataCollector type)
        {
            return type == DataCollector.RedisInfo;
        }

        public override string ChartName { get { return "Commands processed"; } }
    }
}
