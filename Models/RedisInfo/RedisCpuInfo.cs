using MPE.SS.Logic.Attributes;

namespace MPE.SS.Models.RedisInfo
{
    public class RedisCpuInfo
    {
        [RedisField("used_cpy_sys")]
        public double UsedCpuSystem { get; set; }
        [RedisField("used_cpu_user")]
        public double UsedCpuUser { get; set; }
        [RedisField("used_cpu_sys_children")]
        public double UsedCpuChildren { get; set; }
        [RedisField("used_cpu_user_children")]
        public double UsedCpuUserChildren { get; set; }
    }
}