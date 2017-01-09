using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Enums;
using MPE.SS.Interfaces;
using MPE.SS.Models;

namespace MPE.SS.Logic.Alerts.Notifiers
{
    internal class ConsoleNotifier : INotifier
    {
        public List<AlertLevel> AlertLevels
        {
            get
            {
                return Enum.GetValues(typeof(AlertLevel)).Cast<AlertLevel>().ToList();
            }
        }

        public void Notify(Notification notification)
        {
            Console.WriteLine();
            Console.WriteLine("!!! ALERT !!!");
            Console.WriteLine("!!! {0} !!!", notification.AlertLevel);
            Console.WriteLine("!!! {0} !!!", notification.Message);
            Console.WriteLine("!!! ALERT !!!");
            Console.WriteLine();
        }
    }
}
