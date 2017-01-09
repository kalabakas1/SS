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
    internal class ServerValidator : ValidatorBase, IValidator<Server, ReportItem>
    {
        private IBuilder<ReportItem> _reportItemBuilder;
        private List<IValidator<Credential, ReportItem>> _credentialValidators;
        public ServerValidator(
            IBuilder<ReportItem> reportItemBuilder,
            IEnumerable<IValidator<Credential, ReportItem>> credentialValidators)
        {
            _reportItemBuilder = reportItemBuilder;
            _credentialValidators = credentialValidators.ToList();
        }

        public bool IsValid(Server obj)
        {
            ReportItem report;
            return IsValid(obj, out report);
        }

        public bool IsValid(Server obj, out ReportItem message)
        {
            message = _reportItemBuilder.Build();

            if (string.IsNullOrEmpty(obj.CredentialAlias))
                message.Add(ReportItemState.Failure, "Credential alias not specified");

            if (string.IsNullOrEmpty(obj.DisplayName))
                message.Add(ReportItemState.Failure, "Display name is null or empty");

            if (string.IsNullOrEmpty(obj.Name))
                message.Add(ReportItemState.Failure, "Name is null or empty");

            ValidateObj(obj.Credential, _credentialValidators, x => x.Alias, message);

            message.Update();
            return message.State == ReportItemState.Success;
        }
    }
}
