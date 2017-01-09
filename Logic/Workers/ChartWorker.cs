using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MPE.SS.Interfaces;
using MPE.SS.Logic.Configurations;
using MPE.SS.Models;
using NLog;

namespace MPE.SS.Logic.Workers
{
    internal class ChartWorker : IWorker
    {
        private Thread _updateChartDataThread;
        private IRepository<Report> _reportRepository;
        private IReportRender _reportRender;

        public ChartWorker(
            IRepository<Report> reportRepository,
            IReportRender reportRender)
        {
            _reportRepository = reportRepository;
            _reportRender = reportRender;
        }
        public void Start()
        {
            _updateChartDataThread = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        var report = _reportRepository.GetAll().OrderByDescending(x => x.CreatedOn).FirstOrDefault();
                        _reportRender.RenderReport(report);
                    }
                    catch (Exception e)
                    {
                        AppConfiguration.Logger.Log(LogLevel.Fatal, e);
                    }
                    Thread.Sleep(AppConfiguration.Configuration.ChartUpdateIntervalMin * 60000);
                }
            });
            _updateChartDataThread.Start();
        }

        public void Stop()
        {
            try
            {
                _updateChartDataThread.Abort();
            }
            catch { }
        }
    }
}
