using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Enums;

namespace MPE.SS.Models
{
    public class ReportItem : FileEntity
    {
        public ReportItem()
        {
            ReportItems = new List<ReportItem>();
        }

        public void Add(ReportItemState reportItemState, string message, string elaboration = null)
        {
            ReportItems.Add(new ReportItem
            {
                State = reportItemState,
                Header = message,
                Elaboration =  elaboration
            });
        }

        public void Add(ReportItem item)
        {
            ReportItems.Add(item);
        }

        public ReportItem Create()
        {
            return Create("");
        }

        public ReportItem Create(string message)
        {
            var item = new ReportItem
            {
                Header = message
            };
            ReportItems.Add(item);
            return item;
        }

        public void Update()
        {
            if (ReportItems != null && ReportItems.Any())
            {
                var deleteItems = new List<ReportItem>(ReportItems.Where(x => x != null && x.Delete));
                foreach (var reportItem in deleteItems)
                    ReportItems.Remove(reportItem);

                State = CalculateState();
            }
        }

        private ReportItemState CalculateState()
        {
            if (ReportItems.Any(x => x != null && x.State == ReportItemState.Failure))
                return ReportItemState.Failure;

            if(ReportItems.Any(x => x != null && x.State == ReportItemState.Warning))
                return ReportItemState.Warning;

            return ReportItemState.Success;
        }

        public ReportItemState State { get; set; }
        public string Header { get; set; }
        public string Elaboration { get; set; }
        public bool Delete { get; set; }
        public List<ReportItem> ReportItems { get; set; }
    }
}
