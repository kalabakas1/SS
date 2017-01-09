using System;
using MPE.SS.Models;
using System.Collections.Generic;
using System.Linq;
using MPE.SS.Enums;
using MPE.SS.Interfaces;
using MPE.SS.Logic.Configurations;
using MPE.SS.Models.Graphs;
using Newtonsoft.Json;

namespace MPE.SS.Logic.Alerts
{
    internal class AlertTestExecutor<T> : IAlertTest
        where T : IChartDataGenerator
    {
        protected IChartDataGenerator _chartDataGenerator;
        protected INotifierService _notifierService;
        protected IBuilder<Notification> _notificationBuilder;
        public AlertTestExecutor(
            T chartGenerator,
            INotifierService notifierService,
            IBuilder<Notification> notificationBuilder)
        {
            _chartDataGenerator = chartGenerator;
            _notifierService = notifierService;
            _notificationBuilder = notificationBuilder;
        }

        public string ChartName
        {
            get { return _chartDataGenerator.ChartName; }
        }

        public void CheckAlert(Server server, Alert alert)
        {
            var chart = _chartDataGenerator.GenerateChart(server.Name);
            var dataSet = chart.DataSets.FirstOrDefault(x => x.Label == alert.Dataset);

            if (alert.Thresholds == null
                || !alert.Thresholds.Any()
                || dataSet?.Points == null
                || !dataSet.Points.Any())
                return;

            ChechIfDataHasBeenReceived(server, alert, dataSet);

            var thresholds = alert.Thresholds.OrderByDescending(x => x.Level);
            foreach (var alertThreshold in thresholds)
            {
                var dataPoints = dataSet.Points.OrderByDescending(x => x.Label).Take(alertThreshold.DurationInMin);
                if (dataPoints.All(x => double.Parse(x.Value.ToString()) >= alertThreshold.Threshold))
                {
                    _notifierService.Notify(
                        _notificationBuilder
                            .Where(x => x.AlertLevel = alertThreshold.Level)
                            .Where(x => x.Message = GenerateMessage(server, alert, alertThreshold)).Build());
                    break;
                }
            }
        }

        private string GenerateMessage(Server server, Alert alert, AlertThreshold thresHold)
        {
            return string.Format("{0} - {1} - {2} - {3}", server.Name, alert.Alias.PadRight(7), alert.Dataset, thresHold.Level);
        }

        private void ChechIfDataHasBeenReceived(Server server, Alert alert, DataSet dataSet)
        {
            if (dataSet?.Points?.Any() == true)
            {
                var newest = dataSet.Points.Max(x => DateTime.Parse(x.Label));
                if(newest <= AppConfiguration.Configuration.StartedAt.AddMinutes(-5))
                    _notifierService.Notify(_notificationBuilder.Where(x => x.AlertLevel = AlertLevel.Error)
                        .Where(x => x.Message = string.Format("{0} - {1} - {2}", server.Name, alert.Alias.PadRight(7), "No contact made to server...")).Build());
            }
        }
    }
}
