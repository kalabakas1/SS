using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MPE.SS.Constants;
using MPE.SS.Enums;
using MPE.SS.Interfaces;
using MPE.SS.Logic.Configurations;
using MPE.SS.Models;
using NLog;

namespace MPE.SS.Logic.Conductors
{
    internal class ServerConductor : IServerConductor
    {
        private List<IServerTest> _serverTests;
        private List<IDataCollector<Server>> _dataCollectors;
        public ServerConductor(
            IEnumerable<IServerTest> serverTest,
            IEnumerable<IDataCollector<Server>> dataCollectors)
        {
            _serverTests = serverTest.ToList();
            _dataCollectors = dataCollectors.ToList();
        }

        public Task ExecuteTests(Server server, ReportItem context)
        {
            context.Header = string.Format("{0} - {1}", server.Label, server.Name);

            var testTask = new Task(() =>
            {
                Parallel.ForEach(_serverTests, serverTest =>
                {
                    if (server.IgnoreStep(serverTest.Name))
                        return;

                    var subContext = context.Create(serverTest.Name);
                    try
                    {
                        serverTest.Run(server, subContext);
                    }
                    catch (Exception e)
                    {
                        subContext.Elaboration = e.Message;
                        subContext.State = ReportItemState.Failure;

                        AppConfiguration.Logger.Log(LogLevel.Fatal, e);
                    }
                });
                context.Update();
            });
            testTask.Start();

            return testTask;
        }

        public Task ExecuteDataCollection(Server server)
        {
            var dataTask = new Task(() =>
            {
                    Parallel.ForEach(_dataCollectors, dataCollector =>
                    {
                        if (!server.IgnoreStep(dataCollector.Name))
                            dataCollector.Collect(server);
                    });
            });
            dataTask.Start();
            return dataTask;
        }
    }
}
