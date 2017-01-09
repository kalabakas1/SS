using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Interfaces;
using MPE.SS.Models;

namespace MPE.SS.Logic.Validators
{
    internal abstract class ValidatorBase
    {
        protected void Validate<T>(string validationName, List<T> objs, List<IValidator<T, ReportItem>> validators, Func<T, string> label, ReportItem context)
        {
            if (objs != null && objs.Any())
            {
                var objContext = context.Create(validationName);
                foreach (var obj in objs)
                {
                    ValidateObj(obj, validators, label, objContext);
                }
                objContext.Update();
            }
        }

        protected void ValidateObj<T>(
            T obj,
            List<IValidator<T, ReportItem>> validators,
            Func<T, string> label,
            ReportItem context)
        {
            var specificObjContext = context.Create(label.Invoke(obj));
            foreach (var validator in validators)
            {
                ReportItem validation;
                if (!validator.IsValid(obj, out validation))
                    specificObjContext.Add(validation);
            }
            specificObjContext.Update();
        }
    }
}
