using System;

namespace MPE.SS.Models.ServerInfo
{
    public class SystemEvent
    {
        public string MachineName { get; set; }
        public object[] Data { get; set; }
        public int Index { get; set; }
        public string Category { get; set; }
        public int CategoryNumber { get; set; }
        public long EventId { get; set; }
        public int EntryType { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public long InstanceId { get; set; }
        public DateTime TimeGenerated { get; set; }
        public DateTime TimeWritten { get; set; }
        public object UserName { get; set; }
        public object Site { get; set; }
        public object Container { get; set; }
    }
}
