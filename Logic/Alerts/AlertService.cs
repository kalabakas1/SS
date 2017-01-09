using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Interfaces;
using MPE.SS.Models;

namespace MPE.SS.Logic.Alerts
{
    internal class AlertService : IAlertService
    {
        private List<IAlertTest> _alertTests;
        private static object _lock = new object();

        public AlertService(
            IEnumerable<IAlertTest> alertTests)
        {
            _alertTests = alertTests.ToList();
        }
        public void TestServerForAlerts(Server server)
        {
            if (server.Alerts == null || !server.Alerts.Any())
            {
                return;
            }

            lock (_lock)
            {
                foreach (var serverAlert in server.Alerts)
                {
                    var alerts = _alertTests.Where(x => x.ChartName == serverAlert.Alias);
                    foreach (var alertTest in alerts)
                    {
                        alertTest.CheckAlert(server, serverAlert);
                    }
                }
            }
        }
    }
}
