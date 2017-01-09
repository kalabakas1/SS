using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Interfaces;
using MPE.SS.Models;
using MPE.SS.Models.ServerInfo;
using Newtonsoft.Json;

namespace MPE.SS.Logic.DataCollectors
{
    internal class ServerUtilizationCollector : IDataCollector<Server>
    {
        private PowerShellService _shellService;
        private IRepository<ServerUtilization> _repository;

        public ServerUtilizationCollector(
            IRepository<ServerUtilization> repository)
        {
            _repository = repository;
        }

        public string Name { get { return "ServerUtilizationCollector"; } }

        public int SleepIntervalInMils { get { return 5000; } }

        public void Collect(Server server)
        {
            _shellService = new PowerShellService(server.Name, server.Credential.Username, server.Credential.Password);

            _shellService.Invoke(shell =>
            {
                ServerUtilization result = null;
                try
                {
                    _shellService.LoadFilesIntoShell(shell, new List<string>
                    {
                            "Get-Server-Utilization.ps1"
                    });

                    var cmdResult = shell.Invoke();
                    result = JsonConvert.DeserializeObject<ServerUtilization>(cmdResult.First().ToString());
                }
                catch
                {
                    result = new ServerUtilization
                    {
                        NetworkUtilization = new NetworkUtilization()
                    };
                }

                if (result != null)
                {
                    if (result.NetworkUtilization == null)
                    {
                        shell.Commands.Clear();
                        shell.AddScript("Restart-Service winmgmt -Force");
                        shell.Invoke();
                    }
                    result.Reference = server.Name;
                    _repository.Save(result);
                }
            });
        }
    }
}
