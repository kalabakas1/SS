using System.Collections.Generic;
using MPE.SS.Constants;
using MPE.SS.Enums;
using MPE.SS.Interfaces;
using MPE.SS.Models.Graphs;
using MPE.SS.Models.MongoDB;

namespace MPE.SS.Logic.Charts.Mongo
{
    internal class MongoNetworkChart : ChartAbstractBase<MongoInfo>
    {
        public MongoNetworkChart(
            IRepository<MongoInfo> mongoInfoRepositoy,
            IBuilder<Chart> chartBuilder,
            IBuilder<DataSet> dataSetBuilder,
            IBuilder<Point> point,
            IBaselineService baselineService)
            : base(chartBuilder, dataSetBuilder, point, mongoInfoRepositoy, baselineService)
        {
        }

        protected override Chart DefineChart(List<MongoInfo> data, string serverName)
        {
            _chartBuilder.Where(x => x.Server = serverName)
                   .Where(x => x.Name = ChartName)
                   .Where(x => x.DataCollector = DataCollector.MongoDbInfo);

            var networkIn =
                PeakChart(CreateDataSet(data, DatasetNames.KbReceived, new Color(153, 255, 51),
                    z => (int) (z.Network.BytesIn / 1024)));
            var networkOut =
                PeakChart(CreateDataSet(data, DatasetNames.KbSent, new Color(193, 66, 66), z => (int) (z.Network.BytesOut / 1024)));

            UpdateBaseline(ChartAlias.MongoNetworkIn, serverName, networkIn);
            UpdateBaseline(ChartAlias.MongoNetworkOut, serverName, networkOut);

            _chartBuilder.Where(x => x.DataSets = new List<DataSet>
                {
                    networkIn,
                    CreateBaseline(networkIn, ChartAlias.MongoNetworkIn, serverName, DatasetNames.KbReceived),
                    networkOut,
                    CreateBaseline(networkOut, ChartAlias.MongoNetworkOut, serverName, DatasetNames.KbSent),
                });
            return _chartBuilder.Build();
        }

        public override bool CanHandle(DataCollector type)
        {
            return type == DataCollector.MongoDbInfo;
        }

        public override string ChartName { get { return "Network"; } }
    }
}
