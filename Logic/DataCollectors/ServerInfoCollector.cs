using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Enums;
using MPE.SS.Interfaces;
using MPE.SS.Logic.Configurations;
using MPE.SS.Logic.Extensions;
using MPE.SS.Models;
using MPE.SS.Models.ServerInfo;
using Newtonsoft.Json;

namespace MPE.SS.Logic.DataCollectors
{
    internal class ServerInfoCollector : IDataCollector<Server>
    {
        private PowerShellService _shellService;
        private IRepository<ServerInfo> _serverInfoRepository;
        public ServerInfoCollector(
            IRepository<ServerInfo> serverInfoRepository)
        {
            _serverInfoRepository = serverInfoRepository;
        }

        public string Name { get { return "ServerInfoCollector"; } }

        public int SleepIntervalInMils { get { return 30000; } }

        public void Collect(Server server)
        {
            _shellService = new PowerShellService(server.Name, server.Credential.Username, server.Credential.Password);

            var files = new List<string>
            {
                "SystemInfo.ps1"
            };

            _shellService.Invoke(shell =>
            {
                _shellService.LoadFilesIntoShell(shell, files);
                var results = shell.Invoke();

                var convertedResult = _shellService.ConvertResult(results.ToList(), obj =>
                {
                    return JsonConvert.DeserializeObject<ServerInfo>(obj.ToString());
                });

                var result = convertedResult != null ? convertedResult.FirstOrDefault() : null;
                if (result != null)
                {
                    result.Reference = server.Name;
                    server.CollectedData.AddOrUpdate(DataCollector.SystemInfo, result);
                    _serverInfoRepository.Save(result);
                }

            }, AppConfiguration.Configuration.AppState == AppState.Release);
        }
    }
}
