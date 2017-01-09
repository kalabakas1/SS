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
    internal class HaProxyResponseChart : ChartAbstractBase<HapInfo>
    {
        public HaProxyResponseChart(
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

            var http1 = PeakChart(CreateDataSet(data, DatasetNames.Http1xx, new Color(153, 255, 51), x => x.Servers.Sum(z => z.NumberOf100Responses)));
            var http2 = PeakChart(CreateDataSet(data, DatasetNames.Http2xx, new Color(136, 45, 96), x => x.Servers.Sum(z => z.NumberOf200Responses)));
            var http3 = PeakChart(CreateDataSet(data, DatasetNames.Http3xx, new Color(170, 57, 57), x => x.Servers.Sum(z => z.NumberOf300Responses)));
            var http4 = PeakChart(CreateDataSet(data, DatasetNames.Http4xx, new Color(122, 159, 53), x => x.Servers.Sum(z => z.NumberOf400Responses)));
            var http5 = PeakChart(CreateDataSet(data, DatasetNames.Http5xx, new Color(45, 136, 45), x => x.Servers.Sum(z => z.NumberOf500Responses)));

            _chartBuilder.Where(x => x.DataSets = new List<DataSet>
            {
                http1,
                http2,
                http3,
                http4,
                http5
            });

            return _chartBuilder.Build();
        }

        public override string ChartName { get { return "Response types"; } }

        public override bool CanHandle(DataCollector type)
        {
            return type == DataCollector.HaProxyInfo;
        }
    }
}
