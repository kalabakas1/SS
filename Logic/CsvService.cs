using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Interfaces;
using MPE.SS.Logic.Attributes;
using MPE.SS.Logic.Configurations;
using MPE.SS.Models.HaProxy;
using NLog;

namespace MPE.SS.Logic
{
    internal class CsvService<T> : ICsvService<T>
    {
        private char _fieldDelimiter = ',';
        private IBuilder<T> _builder;
        public CsvService(
            IBuilder<T> builder)
        {
            _builder = builder;
        }

        public T ParseLine(string line)
        {
            var properties = GetPropertiesWithAttribute(typeof(T));
            var obj = _builder.Build();
            var data = line.Split(_fieldDelimiter);

            for (int i = 0; i < data.Length; i++)
            {
                var property = properties.FirstOrDefault(x => x.GetCustomAttribute<CsvPosition>().Position == i);
                if (property != null)
                {
                    try
                    {
                        property.SetValue(obj, Convert.ChangeType(data[i], property.PropertyType));
                    }
                    catch (Exception e)
                    {
                        AppConfiguration.Logger.Log(LogLevel.Fatal, e);
                    }
                }
            }

            return obj;
        }

        private List<PropertyInfo> GetPropertiesWithAttribute(Type type)
        {
            return type.GetProperties().Where(x => x.GetCustomAttribute<CsvPosition>() != null).ToList();
        }
    }
}
