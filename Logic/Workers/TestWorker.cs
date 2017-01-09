using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MPE.SS.Exceptions;
using MPE.SS.Interfaces;
using MPE.SS.Logic.Configurations;
using MPE.SS.Models;
using NLog;

namespace MPE.SS.Logic.Workers
{
    internal class TestWorker : IWorker
    {
        private IBuilder<Report> _reportBuilder;
        private IConfigurationService _configurationService;
        private IConductor _conductor;
        private IReportRender _reportRender;
        private IRepository<Report> _reportRepository;

        private Thread _testWorkerThread;
        public TestWorker(
            IBuilder<Report> reportBuilder,
            IConfigurationService configurationService,
            IConductor conductor,
            IReportRender reportRender,
            IRepository<Report> reportRepository)
        {
            _reportBuilder = reportBuilder;
            _configurationService = configurationService;
            _conductor = conductor;
            _reportRender = reportRender;
            _reportRepository = reportRepository;
        }

        public void Start()
        {
            _testWorkerThread = new Thread(() =>
            {
                var configurationAliases = _configurationService.GetConfigurationAliases();

                Parallel.ForEach(configurationAliases, configurationAlias =>
                {
                    try
                    {
                        var configuration = _configurationService.GetConfiguration(configurationAlias);

                        var start = DateTime.Now;
                        var report = _reportBuilder.Where(x => x.Configuration = configuration)
                            .Where(x => x.Start = start)
                            .Where(x => x.Alias = configuration.Alias)
                            .Where(x => x.Header = configuration.Header)
                            .Where(x => x.Description = configuration.Description);

                        var reportItem = _conductor.ExecuteTests(configuration);

                        var end = DateTime.Now;
                        report.Where(x => x.End = end)
                            .Where(x => x.ReportItem = reportItem);

                        var finalReport = report.Build();
                        _reportRepository.Save(finalReport);

                        _reportRender.RenderReport(finalReport);
                    }
                    catch (InvalidValidationException e)
                    {
                        AppConfiguration.Logger.Log(LogLevel.Fatal, e);
                    }

                    Thread.Sleep(AppConfiguration.Configuration.TestExecutionIntervalInMin * 60000);
                });
            });

            _testWorkerThread.Start();
        }

        public void Stop()
        {
            try
            {
                _testWorkerThread.Abort();
            }
            catch { }
        }
    }
}
