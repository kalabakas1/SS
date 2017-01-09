using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPE.SS.Models.ServerInfo
{
    public class ServerUtilization : Data<Server>
    {
        public double RamUsage { get; set; }
        public double RamTotal { get; set; }
        public int? CpuUsage { get; set; }
        public Disk[] Disks { get; set; }
        public NetworkUtilization NetworkUtilization { get; set; }

        public override string GenerateId()
        {
            return string.Format("{0}-{1}-{2}", GetType().Name, Reference, Guid.NewGuid());
        }
    }
}
