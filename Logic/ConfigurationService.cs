using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Exceptions;
using MPE.SS.Interfaces;
using MPE.SS.Models;
using Newtonsoft.Json;

namespace MPE.SS.Logic
{
    internal class ConfigurationService : IConfigurationService
    {
        private IBuilder<Configuration> _configurationBuilder;
        private IValidator<Configuration, ReportItem> _validator;

        public ConfigurationService(
            IBuilder<Configuration> configurationBuilder,
            IValidator<Configuration, ReportItem> validator)
        {
            _configurationBuilder = configurationBuilder;
            _validator = validator;
        }

        public Configuration GetConfiguration(string alias)
        {
            Configuration configuration = null;
            try
            {
                var profile = File.ReadAllText(string.Format("./{0}/{1}.json", Constants.SystemConstant.ProfileFolder, alias));
                configuration = JsonConvert.DeserializeObject<Configuration>(profile);

                foreach (var configurationServer in configuration.Servers)
                {
                    configurationServer.Credential =
                        configuration.Credentials.FirstOrDefault(x => x.Alias == configurationServer.CredentialAlias);
                }
                configuration.Servers = configuration.Servers.OrderBy(x => x.Name).ToList();
            }
            catch
            {
                configuration = _configurationBuilder.Build();
            }

            ReportItem item;
            if (!_validator.IsValid(configuration, out item))
                throw new InvalidValidationException { Report = item };

            return configuration;
        }

        public List<string> GetConfigurationAliases()
        {
            try
            {
                var files = Directory.GetFiles(string.Format("./{0}/", Constants.SystemConstant.ProfileFolder))
                    .Select(x =>
                    {
                        var fileInfo  =new FileInfo(x);
                        return fileInfo.Name.Replace(fileInfo.Extension, string.Empty);
                    });
                return files.ToList();
            }
            catch
            {
                return new List<string>();
            }
        } 
    }
}
