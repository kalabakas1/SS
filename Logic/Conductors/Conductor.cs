using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MPE.SS.Interfaces;
using MPE.SS.Models;

namespace MPE.SS.Logic.Conductors
{
    internal class Conductor : IConductor
    {
        private IBuilder<ReportItem> _reportItemBuilder;
        private IServerConductor _serverConductor;
        private IRequestConductor _requestConductor;
        public Conductor(
            IBuilder<ReportItem> reportItemBuilder,
            IServerConductor serverConductor,
            IRequestConductor requestConductor)
        {
            _reportItemBuilder = reportItemBuilder;
            _serverConductor = serverConductor;
            _requestConductor = requestConductor;
        }

        public ReportItem ExecuteTests(Configuration configuration)
        {
            var context = _reportItemBuilder.Build();
            var serverTestTask = new Task(() =>
            {
                var serverContexts = context.Create("Server tests");
                var tasks = new List<Task>();
                foreach (var configurationServer in configuration.Servers ?? new List<Server>())
                {
                    var serverContext = serverContexts.Create();
                    var task = _serverConductor.ExecuteTests(configurationServer, serverContext);
                    tasks.Add(task);
                    //task.Wait();
                }
                Task.WaitAll(tasks.ToArray());
                serverContexts.Update();
            });

            var requestTests = new Task(() =>
            {
                var requestContexts = context.Create("Request tests");
                foreach (var configurationRequest in configuration.Requests ?? new List<Request>())
                {
                    var requestContext = requestContexts.Create();
                    _requestConductor.ExecuteTests(configurationRequest, requestContext);
                    requestContext.Update();
                }
                requestContexts.Update();
            });

            serverTestTask.Start();
            requestTests.Start();
            serverTestTask.Wait();
            requestTests.Wait();

            context.Update();

            return context;
        }

        public List<Task> ExecuteDataCollection(Configuration configuration)
        {
            var tasks = new List<Task>();
            foreach (var configurationServer in configuration.Servers)
            {
                tasks.Add(_serverConductor.ExecuteDataCollection(configurationServer));
            }
            return tasks;
        }
    }
}