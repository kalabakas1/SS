using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Interfaces;
using MPE.SS.Logic;
using MPE.SS.Logic.Alerts;
using MPE.SS.Logic.Alerts.Notifiers;
using MPE.SS.Logic.Builders;
using MPE.SS.Logic.Charts;
using MPE.SS.Logic.Charts.HaProxy;
using MPE.SS.Logic.Charts.Mongo;
using MPE.SS.Logic.Charts.Redis;
using MPE.SS.Logic.Charts.Servers;
using MPE.SS.Logic.Conductors;
using MPE.SS.Logic.Configurations;
using MPE.SS.Logic.DataCollectors;
using MPE.SS.Logic.Rendering;
using MPE.SS.Logic.Rendering.Graphs;
using MPE.SS.Logic.Repositories;
using MPE.SS.Logic.TestCures;
using MPE.SS.Logic.Tests;
using MPE.SS.Logic.Tests.PowerShell;
using MPE.SS.Logic.Tests.Requests;
using MPE.SS.Logic.Validators;
using MPE.SS.Logic.Workers;
using MPE.SS.Models;
using MPE.SS.Models.Graphs;
using MPE.SS.Models.HaProxy;
using MPE.SS.Models.MongoDB;
using MPE.SS.Models.RedisInfo;
using MPE.SS.Models.ServerInfo;
using SimpleInjector;
using SimpleInjector.Extensions.ExecutionContextScoping;


namespace MPE.SS
{
    internal class App : IApp
    {
        private Container _container;

        public Container GetContainer()
        {
            if (_container == null)
                Configure();
            return _container;
        }

        public void Configure()
        {
            _container = new Container();

            _container.Register<IMainWorker, Worker>();
            _container.RegisterCollection<IWorker>(new List<Type>
            {
                typeof(GarbageCollectorWorker),
                typeof(ChartWorker),
                typeof(TestWorker),
                typeof(DataCollectorWorker),
                typeof(AlertWorker)
            });

            _container.Register<IBuilder<Configuration>, Builder<Configuration>>();
            _container.Register<IBuilder<ReportItem>, Builder<ReportItem>>();
            _container.Register<IBuilder<Report>, Builder<Report>>();
            _container.Register<IBuilder<Graph>, Builder<Graph>>();
            _container.Register<IBuilder<Node>, Builder<Node>>();
            _container.Register<IBuilder<Chart>, Builder<Chart>>();
            _container.Register<IBuilder<DataSet>, Builder<DataSet>>();
            _container.Register<IBuilder<Point>, Builder<Point>>();
            _container.Register<IBuilder<Baseline>, Builder<Baseline>>();
            _container.Register<IBuilder<Notification>, Builder<Notification>>();
            _container.Register<IBuilder<HapInfo>, Builder<HapInfo>>();
            _container.Register<IBuilder<HapServer>, Builder<HapServer>>();

            _container.Register<IConfigurationService, ConfigurationService>();
            _container.Register<IApplicationConfiguration, ApplicationConfiguration>();

            _container.Register<IGraphRenderService, GraphRenderService>();
            _container.RegisterCollection<IChartDataGenerator>(new List<Type>
            {
                typeof(RedisMemoryChart),
                typeof(ServerMemoryChart),
                typeof(RedisCommandChart),
                typeof(ServerCpuChart),
                typeof(ServerNetworkChart),
                typeof(MongoActionChart),
                typeof(MongoNetworkChart),
                typeof(HaProxyResponseChart),
                typeof(HaProxyNetworkChart),
            });

            _container.RegisterCollection<IAlertTest>(new List<Type>
            {
                typeof(AlertTestExecutor<ServerMemoryChart>),
                typeof(AlertTestExecutor<ServerCpuChart>),
                typeof(AlertTestExecutor<ServerNetworkChart>)
            });

            _container.RegisterCollection<INotifier>(new List<Type>
            {
                typeof(ConsoleNotifier),
                typeof(FileNotifier)
            });

            _container.RegisterCollection<IServerTest>(new List<Type>
            {
                typeof(AppPoolAlive),
                typeof(ServiceAlive),
                typeof(ScheduledTaskAlive),
                typeof(PortOpenTest),
                typeof(WindowsFeatureTest)
            });
            _container.Register<ITestCure<AppPoolAlive, string>, AppPoolStartCure>();
            _container.Register<ITestCure<ServiceAlive, string>, ServiceStartCure>();
            _container.Register<ITestCure<ScheduledTaskAlive, string>, NullCure<ScheduledTaskAlive, string>>();
            _container.Register<ITestCure<PortOpenTest, Connection>, NullCure<PortOpenTest, Connection>>();
            _container.Register<ITestCure<WindowsFeatureTest, string>, WindowsFeatureInstallCure>();

            _container.RegisterCollection<IRequestTest>(new List<Type>
            {
                typeof(StatusRequestTest),
                typeof(GzipRequestTest),
                typeof(SslRequestTest)
            });

            _container.RegisterCollection<IDataCollector<Server>>(new List<Type>
            {
                typeof(ServerInfoCollector),
                typeof(RedisInfoCollector),
                typeof(ServerUtilizationCollector),
                typeof(MongoInfoCollector),
                typeof(HaProxyCollector)
            });

            _container.Register<IValidator<Configuration, ReportItem>, ConfigurationValidator>();
            _container.RegisterCollection(typeof(IValidator<Server, ReportItem>), new List<Type>
            {
                typeof(ServerValidator),
            });
            _container.RegisterCollection(typeof(IValidator<Credential, ReportItem>), new List<Type>
            {
                typeof(CredentialValidator)
            });
            _container.RegisterCollection(typeof(IValidator<Request, ReportItem>), new List<Type>
            {
                typeof(RequestValidator)
            });

            _container.Register<IRequestConductor, RequestConductor>();
            _container.Register<IServerConductor, ServerConductor>();
            _container.Register<IConductor, Conductor>();

            _container.Register<IReportRender, BootstrapReportRender>();

            _container.Register<IFileService, FileService>();
            _container.Register<IBaselineService, BaselineService>();
            _container.Register<IAlertService, AlertService>();
            _container.Register<INotifierService, NotifierService>();
            _container.Register<ICsvService<HapServer>, CsvService<HapServer>>();

            _container.Register<IRepository<Report>, LiteDbRepository<Report>>(Lifestyle.Singleton);
            _container.Register<IRepository<ServerInfo>, LiteDbRepository<ServerInfo>>(Lifestyle.Singleton);
            _container.Register<IRepository<RedisInfo>, LiteDbRepository<RedisInfo>>(Lifestyle.Singleton);
            _container.Register<IRepository<ServerUtilization>, LiteDbRepository<ServerUtilization>>(Lifestyle.Singleton);
            _container.Register<IRepository<MongoInfo>, LiteDbRepository<MongoInfo>>(Lifestyle.Singleton);
            _container.Register<IRepository<Baseline>, LiteDbRepository<Baseline>>(Lifestyle.Singleton);
            _container.Register<IRepository<HapInfo>, LiteDbRepository<HapInfo>>(Lifestyle.Singleton);

            _container.Verify();

            AppConfiguration.Configuration = _container.GetInstance<IApplicationConfiguration>();
        }
    }
}
