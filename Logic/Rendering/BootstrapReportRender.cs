using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Constants;
using MPE.SS.Enums;
using MPE.SS.Interfaces;
using MPE.SS.Logic.Rendering.Graphs;
using MPE.SS.Logic.Rendering.ViewModels;
using MPE.SS.Logic.Repositories;
using MPE.SS.Models;
using MPE.SS.Models.Graphs;
using MPE.SS.Models.HaProxy;
using MPE.SS.Models.MongoDB;
using MPE.SS.Models.RedisInfo;
using MPE.SS.Models.ServerInfo;
using RazorEngine;

namespace MPE.SS.Logic.Rendering
{
    internal class BootstrapReportRender : IReportRender
    {
        private IFileService _fileService;
        private IGraphRenderService _granRenderService;
        private IEnumerable<IChartDataGenerator> _chartDataGenerators;
        private IRepository<ServerInfo> _serverInfoRepository;
        private IRepository<RedisInfo> _redisInfoRepository;
        private IRepository<MongoInfo> _mongoInfoRepository;
        private IRepository<HapInfo> _haProxyInfoRepository;

        private object _lock = new object();

        public BootstrapReportRender(
            IFileService fileService,
            IGraphRenderService graphRenderService,
            IEnumerable<IChartDataGenerator> chartDataGenerators,
            IRepository<ServerInfo> serverInfoRepository,
            IRepository<RedisInfo> redisInfoRepository,
            IRepository<MongoInfo> mongoInfoRepository,
            IRepository<HapInfo> haProxyInfoRepository)
        {
            _fileService = fileService;
            _granRenderService = graphRenderService;
            _chartDataGenerators = chartDataGenerators;
            _serverInfoRepository = serverInfoRepository;
            _redisInfoRepository = redisInfoRepository;
            _mongoInfoRepository = mongoInfoRepository;
            _haProxyInfoRepository = haProxyInfoRepository;
        }

        public void RenderReport(Report report)
        {
            lock (_lock)
            {
                report.LastUpdated = DateTime.Now;

                SetupCollectedData(report);

                report.Graph = _granRenderService.GenerateGraph(report.Configuration);
                report.Charts = RenderCharts(report.Configuration.Servers.ToList()).ToList();
                var template = _fileService.GetEmbeddedRessource("MPE.SS.Logic.Rendering.Views.Report.cshtml");

                var viewModel = new View<Report>();
                viewModel.Links = new List<string>();
                var links = new List<string>
                {
                    "SS.css"
                };
                foreach (var link in links)
                    viewModel.Links.Add(_fileService.GetEmbeddedRessource(SystemConstant.RessourcePrefix + link));

                viewModel.Scripts = new List<string>();
                var scripts = new List<string>
                {
                    "cytoscape-qtip.js",
                    "SS.js"
                };
                foreach (var script in scripts)
                    viewModel.Scripts.Add(_fileService.GetEmbeddedRessource(SystemConstant.RessourcePrefix + script));

                viewModel.Model = report;

                var finalView = Razor.Parse(template, viewModel);

                _fileService.SaveReport(report.Alias, finalView);
            }
        }

        private void SetupCollectedData(Report report)
        {
            foreach (var configurationServer in report.Configuration.Servers)
            {
                HandleCollectedData(_serverInfoRepository, configurationServer, DataCollector.SystemInfo);
                HandleCollectedData(_redisInfoRepository, configurationServer, DataCollector.RedisInfo);
                HandleCollectedData(_mongoInfoRepository, configurationServer, DataCollector.MongoDbInfo);
                HandleCollectedData(_haProxyInfoRepository, configurationServer, DataCollector.HaProxyInfo);
            }
        }

        private void HandleCollectedData<T>(IRepository<T> repository, Server server, DataCollector type)
            where T : Data<Server>
        {
            var info = repository.GetAll()
                                        .Where(x => x.Reference == server.Name)
                                        .OrderByDescending(x => x.CreatedOn)
                                        .FirstOrDefault();

            if (info != null)
            {
                server.CollectedData[type] = info;
            }
        }

        public IEnumerable<Chart> RenderCharts(List<Server> servers)
        {
            var charts = new List<Chart>();
            foreach (var server in servers)
            {
                if (server.CollectedData != null && server.CollectedData.Any())
                {
                    foreach (var data in server.CollectedData)
                    {
                        var chartGenerators = _chartDataGenerators.Where(x => x.CanHandle(data.Key));
                        foreach (var chartDataGenerator in chartGenerators)
                        {
                            var chart = chartDataGenerator.GenerateChart(server.Name);
                            if (chart != null)
                                charts.Add(chart);
                        }
                    }
                }
            }
            return charts.Where(x => x.DataSets.Any() && !string.IsNullOrEmpty(x.Name) && !string.IsNullOrEmpty(x.Server));
        }
    }
}