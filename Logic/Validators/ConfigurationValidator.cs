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
    internal class ConfigurationValidator : ValidatorBase, IValidator<Configuration, ReportItem>
    {
        private IBuilder<ReportItem> _reportItemBuilder;
        private List<IValidator<Server, ReportItem>> _serverValidators;
        private List<IValidator<Request, ReportItem>> _requestValidators;
        public ConfigurationValidator(
            IBuilder<ReportItem> reportItemBuilder,
            IEnumerable<IValidator<Server, ReportItem>> serverValidators,
            IEnumerable<IValidator<Request, ReportItem>> requestValidators)
        {
            _reportItemBuilder = reportItemBuilder;
            _serverValidators = serverValidators.ToList();
            _requestValidators = requestValidators.ToList();
        }

        public bool IsValid(Configuration obj)
        {
            return false;
        }

        public bool IsValid(Configuration obj, out ReportItem message)
        {
            message = _reportItemBuilder.Build();
            Validate("Server validation", obj.Servers, _serverValidators, x => x.Name, message);
            Validate("Request validation", obj.Requests, _requestValidators, x => x.Label, message);
            message.Update();
            return message.State == ReportItemState.Success;
        }
    }
}
