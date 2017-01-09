using System.Collections.Generic;
using MPE.SS.Constants;
using MPE.SS.Enums;
using MPE.SS.Interfaces;
using MPE.SS.Models.Graphs;
using MPE.SS.Models.MongoDB;

namespace MPE.SS.Logic.Charts.Mongo
{
    internal class MongoActionChart : ChartAbstractBase<MongoInfo>
    {
        public MongoActionChart(
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

            var queries = PeakChart(CreateDataSet(data, DatasetNames.Queries, new Color(153, 255, 51), z => z.Opcounters.Query));
            var inserts = PeakChart(CreateDataSet(data, DatasetNames.Inserts, new Color(193, 66, 66), z => z.Opcounters.Insert));
            var updates = PeakChart(CreateDataSet(data, DatasetNames.Updates, new Color(99, 82, 80), z => z.Opcounters.Update));
            var deletes = PeakChart(CreateDataSet(data, DatasetNames.Deletes, new Color(194, 232, 18), z => z.Opcounters.Delete));

            UpdateBaseline(ChartAlias.MongoQueries, serverName, queries);
            UpdateBaseline(ChartAlias.MongoInserts, serverName, inserts);
            UpdateBaseline(ChartAlias.MongoUpdates, serverName, updates);
            UpdateBaseline(ChartAlias.MongoDeletes, serverName, deletes);

            _chartBuilder.Where(x => x.DataSets = new List<DataSet>
                {
                    queries,
                    inserts,
                    updates,
                    deletes,
                });
            return _chartBuilder.Build();
        }

        public override bool CanHandle(DataCollector type)
        {
            return type == DataCollector.MongoDbInfo;
        }

        public override string ChartName { get { return "Actions"; } }
    }
}
