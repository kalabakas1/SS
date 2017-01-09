using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Enums;
using MPE.SS.Interfaces;
using MPE.SS.Models;

namespace MPE.SS.Logic.Validators
{
    internal class RequestValidator : IValidator<Request, ReportItem>
    {
        private IBuilder<ReportItem> _reportItemBuilder;

        public RequestValidator(
            IBuilder<ReportItem> reportItemBuilder)
        {
            _reportItemBuilder = reportItemBuilder;
        }

        public bool IsValid(Request obj)
        {
            ReportItem reportItem;
            return IsValid(obj, out reportItem);
        }

        public bool IsValid(Request obj, out ReportItem message)
        {
            message = _reportItemBuilder.Build();

            if(string.IsNullOrEmpty(obj.Uri))
                message.Add(ReportItemState.Failure, "Uri is null or empty");

            if(string.IsNullOrEmpty(obj.Label))
                message.Add(ReportItemState.Failure, "Label is null or empty");

            try
            {
                WebRequest.CreateHttp(obj.Uri);
            }
            catch
            {
                message.Add(ReportItemState.Failure, "Uri not in correct format");
            }

            try
            {
                var status = (HttpStatusCode) obj.StatusCode;
            }
            catch
            {
                message.Add(ReportItemState.Failure, "StatusCode not valid");
            }
            message.Update();

            return message.State == ReportItemState.Success;
        }
    }
}
