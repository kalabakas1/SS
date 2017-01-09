using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Enums;
using MPE.SS.Interfaces;
using MPE.SS.Models;

namespace MPE.SS.Logic.Validators
{
    internal class CredentialValidator : IValidator<Credential, ReportItem>
    {
        private IBuilder<ReportItem> _reportItemBuilder;
        public CredentialValidator(
            IBuilder<ReportItem> reportItemBuilder)
        {
            _reportItemBuilder = reportItemBuilder;
        }

        public bool IsValid(Credential obj)
        {
            ReportItem report;
            return IsValid(obj, out report);
        }

        public bool IsValid(Credential obj, out ReportItem message)
        {
            message = _reportItemBuilder.Build();
            if(string.IsNullOrEmpty(obj.Alias))
                message.Add(ReportItemState.Failure, "Alias is null or empty");

            if(string.IsNullOrEmpty(obj.Username))
                message.Add(ReportItemState.Failure, "Username is null or empty");

            if(string.IsNullOrEmpty(obj.Password))
                message.Add(ReportItemState.Failure, "Password is null or empty");
            
            message.Update();

            return message.State == ReportItemState.Success;
        }
    }
}
