using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Logic.Attributes;

namespace MPE.SS.Models.RedisInfo
{
    public class RedisClient
    {
        [RedisField("id")]
        public int Id { get; set; }
        [RedisField("addr")]
        public string Address { get; set; }
        [RedisField("fd")]
        public int FileDescriptor { get; set; }
        [RedisField("name")]
        public string Name { get; set; }
        [RedisField("age")]
        public long AgeInSec { get; set; }
        [RedisField("idle")]
        public int IdleInSec { get; set; }
        [RedisField("flags")]
        public string Flags { get; set; }
        [RedisField("db")]
        public int Database { get; set; }
        [RedisField("sub")]
        public int ChannelSubscriptions { get; set; }
        [RedisField("psub")]
        public int PatternMatchingSubscriptions { get; set; }
        [RedisField("multi")]
        public int MultiExecCommands { get; set; }
        [RedisField("qbuf")]
        public int QueryBufferLength { get; set; }
        [RedisField("qbuf-free")]
        public int QueryBufferFreeSpace { get; set; }
        [RedisField("obl")]
        public int OutputBufferLength { get; set; }
        [RedisField("oll")]
        public int OutputListLength { get; set; }
        [RedisField("omem")]
        public string OutputBufferMemoryUsage { get; set; }
        [RedisField("events")]
        public string FileDescriptorEvents { get; set; }
        [RedisField("cmd")]
        public string Command { get; set; }
    }
}
