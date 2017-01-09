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
    internal class ServerNetworkChart : ChartAbstractBase<ServerUtilization>
    {
        private static object _lock = new object();

        public ServerNetworkChart(
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
                data = data.Where(x => x.NetworkUtilization != null).ToList();
                var networkIn = CreateDataSet(data, DatasetNames.KbReceivedPerSec, new Color(),
                    z => (int)z.NetworkUtilization.ReceivedKiloBytesPerSec);
                UpdateBaseline(ChartAlias.ServerNetworkIn, serverName, networkIn);

                var networkOut = CreateDataSet(data, DatasetNames.KbSentPerSec, new Color(193, 66, 66),
                   z => (int)z.NetworkUtilization.SentKiloBytesPerSec);
                UpdateBaseline(ChartAlias.ServerNetworkOut, serverName, networkOut);

                _chartBuilder.Where(x => x.DataSets = new List<DataSet>
                {
                    networkIn,
                    CreateBaseline(networkIn, ChartAlias.ServerNetworkIn, serverName, DatasetNames.KbReceivedPerSec),
                    networkOut,
                    CreateBaseline(networkOut, ChartAlias.ServerNetworkOut, serverName, DatasetNames.KbSentPerSec)
                });

                return _chartBuilder.Build();
            }
        }

        public override string ChartName { get { return "Network utilization on server"; } }
    }
}
