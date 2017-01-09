using System;
using MPE.SS.Models.Graphs;
using Newtonsoft.Json;

namespace MPE.SS.Models.ServerInfo
{
    public class ServerInfo : Data<Server>
    {
        public string ComputerName { get; set; }
        [JsonProperty(PropertyName = "System")]
        public SystemInfo System { get; set; }
        public Disk[] Disk { get; set; }
        public Ip[] Ip { get; set; }
        public Cpu Cpu { get; set; }
        public RAM RAM { get; set; }
        public Bios Bios { get; set; }
        public OS OS { get; set; }
        public SystemEvent[] Events { get; set; }
        
        public override string GenerateId()
        {
            return string.Format("{0}-{1}-{2}", GetType().Name, ComputerName, Guid.NewGuid());
        }
    }
}
