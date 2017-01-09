using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MPE.SS.Constants;
using MPE.SS.Interfaces;
using MPE.SS.Models;

namespace MPE.SS.Logic.Workers
{
    internal class DataCollectorWorker : IWorker
    {
        private IConfigurationService _configurationService;
        private List<IDataCollector<Server>> _dataCollectors;

        private List<Thread> _dataCollectorThreads;

        public DataCollectorWorker(
            IConfigurationService configurationService,
            IEnumerable<IDataCollector<Server>> dataCollectors)
        {
            _dataCollectorThreads = new List<Thread>();

            _configurationService = configurationService;
            _dataCollectors = dataCollectors.ToList();
        }
        public void Start()
        {
            var configurationAliases = _configurationService.GetConfigurationAliases();
            var configurations = configurationAliases.Select(x => _configurationService.GetConfiguration(x)).ToList();
            var servers = configurations.SelectMany(x => x.Servers);

            Parallel.ForEach(servers, server =>
            {
                try
                {
                    foreach (var dataCollector in _dataCollectors)
                    {
                        if (server.IgnoreStep(dataCollector.Name))
                            continue;
                        var thread = new Thread(() =>
                        {
                            while (true)
                            {
                                try
                                {
                                    dataCollector.Collect(server);
                                }
                                catch
                                {

                                }

                                Thread.Sleep(dataCollector.SleepIntervalInMils);
                            }
                        });
                        _dataCollectorThreads.Add(thread);
                        thread.Name = string.Format("{0} # {1}", dataCollector.GetType().Name, server.Name);
                        thread.Start();
                    }
                }
                catch
                {

                }
            });
        }

        public void Stop()
        {
            if (_dataCollectorThreads != null && _dataCollectorThreads.Any())
            {
                foreach (var thread in _dataCollectorThreads)
                {
                    try
                    {
                        thread.Abort();
                    }
                    catch { }
                }
            }
        }
    }
}
