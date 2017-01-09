using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization;
using MPE.SS.Constants;
using MPE.SS.Enums;
using MPE.SS.Interfaces;
using MPE.SS.Logic.Extensions;
using MPE.SS.Models;
using MPE.SS.Models.MongoDB;
using Newtonsoft.Json;

namespace MPE.SS.Logic.DataCollectors
{
    internal class MongoInfoCollector : IDataCollector<Server>
    {
        private PowerShellService _shellService;

        private IRepository<MongoInfo> _mongoInfoRepository;
        public MongoInfoCollector(
            IRepository<MongoInfo> mongoInfoRepository)
        {
            _mongoInfoRepository = mongoInfoRepository;

            if (!BsonClassMap.IsClassMapRegistered(typeof(MongoInfo)))
                BsonClassMap.RegisterClassMap<MongoInfo>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });
        }

        public string Name { get { return "MongoInfoCollector"; } }

        public int SleepIntervalInMils { get { return 30000; } }

        public void Collect(Server server)
        {
            if (!IsValid(server))
                return;

            _shellService = new PowerShellService(server.Name, server.Credential.Username, server.Credential.Password);

            _shellService.Invoke(shell =>
            {
                var username = server.GetAdditional<string>(AdditionalConstant.MongoUsername);
                var password = server.GetAdditional<string>(AdditionalConstant.MongoPassword);
                var database = server.GetAdditional<string>(AdditionalConstant.MongoDatabase);
                var authenticationDb = server.GetAdditional<string>(AdditionalConstant.MongoAuthenticationDatabase);
                var cmd = string.Format(@"(mongo.exe --quiet -eval 'db.runCommand( {{ serverStatus: 1, repl: 0, metrics: 0, locks: 0 }} )' -u '{0}' -p '{1}' -authenticationDatabase '{2}' {3}/{4})", username, password, authenticationDb, server.Name, database);

                shell.AddScript(cmd);

                var result = shell.Invoke();
                if (result != null && result.Any())
                {
                    var resultString = string.Join("\n", result.Select(x => x.ToString()));
                    var resultObj = BsonSerializer.Deserialize<MongoInfo>(resultString);
                    if (resultObj != null)
                    {
                        resultObj.Reference = server.Name;
                        server.CollectedData.AddOrUpdate(DataCollector.MongoDbInfo, resultObj);
                        _mongoInfoRepository.Save(resultObj);
                    }
                }
            });
        }

        private bool IsValid(Server server)
        {
            return server.Services != null
                   && server.Services.Contains("MongoDB")
                   && server.Additionals.ContainsKey(AdditionalConstant.MongoAuthenticationDatabase)
                   && server.Additionals.ContainsKey(AdditionalConstant.MongoDatabase)
                   && server.Additionals.ContainsKey(AdditionalConstant.MongoPassword)
                   && server.Additionals.ContainsKey(AdditionalConstant.MongoUsername);
        }
    }
}
