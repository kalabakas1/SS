using System;
using MPE.SS.Interfaces;
using MPE.SS.Logic.Configurations;
using NLog;
using Topshelf;
using Topshelf.SimpleInjector;

namespace MPE.SS
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new App();

            var host = HostFactory.New(config =>
            {
                config.EnableServiceRecovery(r =>
                {
                    r.OnCrashOnly();
                    r.SetResetPeriod(1);
                    r.RestartService(1);
                });
                config.UseSimpleInjector(app.GetContainer());
                config.Service<IMainWorker>(s =>
                {
                    s.ConstructUsingSimpleInjector();
                    s.WhenStarted(service => service.Start());
                    s.WhenStopped(service => service.Stop());
                });
                config.RunAsLocalSystem();

                config.SetDescription("MPE.SS - Server Surveillance");
                config.SetDisplayName("MPE.SS");
                config.SetServiceName("MPE.SS");
            });

            host.Run();
        }
    }
}