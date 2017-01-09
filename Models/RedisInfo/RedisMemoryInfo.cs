using MPE.SS.Logic.Attributes;

namespace MPE.SS.Models.RedisInfo
{
    public class RedisMemoryInfo
    {
        [RedisField("used_memory")]
        public long UsedMemory { get; set; }
        [RedisField("used_memory_human")]
        public string UsedMemoryFormatted { get; set; }
        [RedisField("used_memory_rss")]
        public long UsedMemoryRss { get; set; }
        [RedisField("used_memory_rss_human")]
        public string UsedMemoryRssFormatted { get; set; }
        [RedisField("used_memory_peak")]
        public long UsedMemoryPeak { get; set; }
        [RedisField("used_memory_peak_human")]
        public string UsedMemoryPeakFormatted { get; set; }
        [RedisField("total_system_memory")]
        public long TotalSystemMemory { get; set; }
        [RedisField("total_system_memory_human")]
        public string TotalSystemMemoryFormatted { get; set; }
        [RedisField("maxmemory")]
        public long MaxMemory { get; set; }
        [RedisField("maxmemory_human")]
        public string MaxMemoryFormatted { get; set; }
        [RedisField("maxmemory_policy")]
        public string MaxMemoryPolicy { get; set; }
        [RedisField("mem_fragmentation_ration")]
        public double MemoryFragmentationRatio { get; set; }
        [RedisField("mem_allocator")]
        public string MemoryAllocator { get; set; }
    }
}