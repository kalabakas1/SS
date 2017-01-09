using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace MPE.SS.Models.MongoDB
{
    public class MongoInfo : Data<Server>
    {
        [BsonElement("host")]
        public string Host { get; set; }
        [BsonElement("advisoryHostFQDNs")]
        public string[] AdvisoryHostFqdNs { get; set; }
        [BsonElement("version")]
        public string Version { get; set; }
        [BsonElement("process")]
        public string Process { get; set; }
        [BsonElement("pid")]
        public int Pid { get; set; }
        [BsonElement("uptime")]
        public int Uptime { get; set; }
        [BsonElement("uptimeMillis")]
        public int UptimeMillis { get; set; }
        [BsonElement("uptimeEstimate")]
        public int UptimeEstimate { get; set; }
        [BsonElement("localTime")]
        public DateTime LocalTime { get; set; }
        [BsonElement("connections")]
        public MongoConnections Connections { get; set; }
        [BsonElement("network")]
        public MongoNetwork Network { get; set; }
        [BsonElement("opcounters")]
        public MongoOpcounters Opcounters { get; set; }
        [BsonElement("opcountersRepl")]
        public MongoOpcounters OpcountersRepl { get; set; }
        [BsonElement("ok")]
        public int Ok { get; set; }
    }
}
