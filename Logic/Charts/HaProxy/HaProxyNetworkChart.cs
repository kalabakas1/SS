using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Constants;
using MPE.SS.Enums;
using MPE.SS.Interfaces;
using MPE.SS.Models.Graphs;
using MPE.SS.Models.HaProxy;

namespace MPE.SS.Logic.Charts.HaProxy
{
    internal class HaProxyNetworkChart : ChartAbstractBase<HapInfo>
    {
        public HaProxyNetworkChart(
            IBuilder<Chart> chartBuilder,
            IBuilder<DataSet> dataSetBuilder,
            IBuilder<Point> pointBuilder,
            IRepository<HapInfo> repository,
            IBaselineService baselineService)
            : base(chartBuilder, dataSetBuilder, pointBuilder, repository, baselineService)
        {
        }

        protected override Chart DefineChart(List<HapInfo> data, string serverName)
        {
            _chartBuilder.Where(x => x.Server = serverName)
                   .Where(x => x.Name = ChartName)
                   .Where(x => x.DataCollector = DataCollector.HaProxyInfo);

            var kbIn = PeakChart(CreateDataSet(data, DatasetNames.KbReceived, new Color(153, 255, 51), x => x.Servers.Sum(z => z.BytesIn / 1024)));
            var kbOut = PeakChart(CreateDataSet(data, DatasetNames.KbSent, new Color(136, 45, 96), x => x.Servers.Sum(z => z.BytesOut / 1024)));

            _chartBuilder.Where(x => x.DataSets = new List<DataSet>
            {
                kbIn,
                kbOut,
            });

            return _chartBuilder.Build();
        }

        public override string ChartName { get { return "Network Kb"; } }

        public override bool CanHandle(DataCollector type)
        {
            return type == DataCollector.HaProxyInfo;
        }
    }
}
