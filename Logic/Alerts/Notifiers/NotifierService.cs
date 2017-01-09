using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Interfaces;
using MPE.SS.Models;

namespace MPE.SS.Logic.Alerts.Notifiers
{
    internal class NotifierService : INotifierService
    {
        private List<INotifier> _notifiers;

        public NotifierService(
            IEnumerable<INotifier> notifiers)
        {
            _notifiers = notifiers.ToList();
        }
        public void Notify(Notification notification)
        {
            foreach (var notifier in _notifiers)
            {
                if(notifier.AlertLevels.Contains(notification.AlertLevel))
                    notifier.Notify(notification);
            }
        }
    }
}
