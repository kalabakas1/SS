using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MPE.SS.Interfaces;
using MPE.SS.Logic.Configurations;
using NLog;

namespace MPE.SS.Logic.Workers
{
    class AlertWorker : IWorker
    {
        private Thread _alertWorker;
        private IConfigurationService _configurationService;
        private IAlertService _alertService;
        public AlertWorker(
            IConfigurationService configurationService,
            IAlertService alertService)
        {
            _configurationService = configurationService;
            _alertService = alertService;
        }
        public void Start()
        {
            _alertWorker = new Thread(() =>
            {
                var configurationAliases = _configurationService.GetConfigurationAliases();
                var configurations = configurationAliases.Select(x => _configurationService.GetConfiguration(x)).ToList();
                var servers = configurations.SelectMany(x => x.Servers).ToList();
                while (true)
                {
                    Parallel.ForEach(servers, server =>
                    {
                        try
                        {
                            _alertService.TestServerForAlerts(server);
                        }
                        catch (Exception e)
                        {
                            AppConfiguration.Logger.Log(LogLevel.Fatal,e);
                        }
                    });
                    Thread.Sleep(AppConfiguration.Configuration.ChartUpdateIntervalMin * 60000);
                }
            });

            _alertWorker.Start();
        }

        public void Stop()
        {
            try
            {
                _alertWorker.Abort();
            }
            catch (Exception e)
            {
                AppConfiguration.Logger.Log(LogLevel.Fatal, e);
            }
        }
    }
}
