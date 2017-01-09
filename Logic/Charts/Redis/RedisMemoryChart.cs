using System.Collections.Generic;
using MPE.SS.Constants;
using MPE.SS.Enums;
using MPE.SS.Interfaces;
using MPE.SS.Models.Graphs;
using MPE.SS.Models.RedisInfo;

namespace MPE.SS.Logic.Charts.Redis
{
    internal class RedisMemoryChart : ChartAbstractBase<RedisInfo>
    {
        private static object _lock = new object();

        public RedisMemoryChart(
            IRepository<RedisInfo> redisInfoRepository,
            IBuilder<Chart> chartBuilder,
            IBuilder<DataSet> dataSetBuilder,
            IBuilder<Point> point,
            IBaselineService baselineService) 
            : base(chartBuilder, dataSetBuilder, point, redisInfoRepository, baselineService)
        {
        }

        protected override Chart DefineChart(List<RedisInfo> data, string serverName)
        {
            lock (_lock)
            {
                _chartBuilder.Where(x => x.Server = serverName)
                    .Where(x => x.Name = ChartName)
                    .Where(x => x.DataCollector = DataCollector.RedisInfo);

                var dataset = CreateDataSet(data, DatasetNames.MemoryUtil, new Color(),
                    z => (int)(z.Memory.UsedMemory / z.Memory.MaxMemory * 100));

                UpdateBaseline(ChartAlias.RedisMemory, serverName, dataset);

                _chartBuilder.Where(x => x.DataSets = new List<DataSet> { dataset }).Build();

                return _chartBuilder.Build();
            }
        }

        public override bool CanHandle(DataCollector type)
        {
            return type == DataCollector.RedisInfo;
        }

        public override string ChartName { get { return "Memory % used by Redis process"; } }
    }
}
