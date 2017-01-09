using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Enums;
using MPE.SS.Interfaces;
using MPE.SS.Models;

namespace MPE.SS.Logic.Alerts.Notifiers
{
    internal class FileNotifier : INotifier
    {
        private static object _lock = new object();
        public List<AlertLevel> AlertLevels
        {
            get
            {
                return Enum.GetValues(typeof(AlertLevel)).Cast<AlertLevel>().ToList();
            }
        }

        public void Notify(Notification notification)
        {
            lock (_lock)
            {
                File.AppendAllLines("NOTIFICATIONS.txt", new List<string>
                {
                    string.Format("!!! ALERT !!! {0} !!! {1} !!! {2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), notification.AlertLevel, notification.Message)
                });
            }
        }
    }
}
