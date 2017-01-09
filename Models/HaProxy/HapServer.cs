using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Logic.Attributes;

namespace MPE.SS.Models.HaProxy
{
    public class HapServer
    {
        [CsvPosition("pxname", 0)]
        public string ProxyName { get; set; }

        [CsvPosition("svname", 1)]
        public string ServiceName { get; set; }

        [CsvPosition("qcur", 2)]
        public int QueuedRequests { get; set; }

        [CsvPosition("qmax", 3)]
        public int MaxQueuedRequets { get; set; }

        [CsvPosition("scur", 4)]
        public int CurrentSessions { get; set; }

        [CsvPosition("smax", 5)]
        public int MaxSessions { get; set; }

        [CsvPosition("slim", 6)]
        public int ConfiguredSessionLimmit { get; set; }

        [CsvPosition("stot", 7)]
        public int CumulativeConnectionCount { get; set; }

        [CsvPosition("bin", 8)]
        public int BytesIn { get; set; }

        [CsvPosition("bout", 9)]
        public int BytesOut { get; set; }

        [CsvPosition("dreq", 10)]
        public int DeniedRequests { get; set; }

        [CsvPosition("dresp", 11)]
        public int DeniedResponses { get; set; }

        [CsvPosition("ereq", 12)]
        public int ErrorRequests { get; set; }

        [CsvPosition("econ", 13)]
        public int ErrorConnections { get; set; }

        [CsvPosition("eres ", 14)]
        public int ErrorResonses { get; set; }

        [CsvPosition("wretr", 15)]
        public int ConnectionRetries { get; set; }

        [CsvPosition("wredis", 16)]
        public int RequestsDispatchedToOtherServer { get; set; }

        [CsvPosition("status", 17)]
        public string Status { get; set; }

        [CsvPosition("weight", 18)]
        public int Weight { get; set; }

        [CsvPosition("act", 19)]
        public int ActiveServers { get; set; }

        [CsvPosition("bck", 20)]
        public int BackupServers { get; set; }

        [CsvPosition("chkfail", 21)]
        public int FailedChecks { get; set; }
        [CsvPosition("chkdown", 22)]
        public int UpDownTransitions { get; set; }
        [CsvPosition("lastchg", 23)]
        public int SecSinceLastUpdownTransition { get; set; }
        [CsvPosition("downtime", 24)]
        public int DowntimeInSec { get; set; }
        [CsvPosition("qlimit", 25)]
        public int ConfiguredMaxQueue { get; set; }
        [CsvPosition("pid", 26)]
        public int ProcessId { get; set; }
        [CsvPosition("iid", 27)]
        public int UniqueProxyId { get; set; }
        [CsvPosition("sid", 28)]
        public int ServerId { get; set; }
        [CsvPosition("throttle", 29)]
        public int ThrottlePercentage { get; set; }
        [CsvPosition("lbtot", 30)]
        public int NumberOfTimesSelected { get; set; }
        [CsvPosition("tracked ", 31)]
        public int TrackingId { get; set; }
        [CsvPosition("type", 32)]
        public int Type { get; set; }
        [CsvPosition("rate", 33)]
        public int SessionsPerSec { get; set; }
        [CsvPosition("rate_lim", 34)]
        public int ConfiguredMaxSessionsPerSec { get; set; }
        [CsvPosition("rate_max", 35)]
        public int MaxSessionsPerSec { get; set; }
        [CsvPosition("check_status", 36)]
        public string LastCheckStatus { get; set; }
        [CsvPosition("check_code", 37)]
        public string CheckCode { get; set; }
        [CsvPosition("check_duration", 38)]
        public int MillDurationForLastCheck { get; set; }
        [CsvPosition("hrsp_1xx", 39)]
        public int NumberOf100Responses { get; set; }
        [CsvPosition("hrsp_2xx", 40)]
        public int NumberOf200Responses { get; set; }
        [CsvPosition("hrsp_3xx", 41)]
        public int NumberOf300Responses { get; set; }
        [CsvPosition("hrsp_4xx", 42)]
        public int NumberOf400Responses { get; set; }
        [CsvPosition("hrsp_5xx", 43)]
        public int NumberOf500Responses { get; set; }
        [CsvPosition("hrsp_other", 44)]
        public int NumberOfOtherResponses { get; set; }
        [CsvPosition("hanafail", 45)]
        public string FailedHealthCheckDetails { get; set; }
        [CsvPosition("req_rate", 46)]
        public int RequestsPerSec { get; set; }
        [CsvPosition("req_rate_max", 47)]
        public int MaxNumberOfRequestsPerSec { get; set; }
        [CsvPosition("req_tot", 48)]
        public int MaxNumberOfRequests { get; set; }
        [CsvPosition("cli_abrt", 49)]
        public int NumberOfTransfersAbortedByClient { get; set; }
        [CsvPosition("srv_abrt", 50)]
        public int NumberOfTransfersAbourtedByServer { get; set; }
        [CsvPosition("comp_in", 51)]
        public int NumberOfResponseBytesInCompressor { get; set; }
        [CsvPosition("comp_out", 52)]
        public int NumberOfResponseBytesEmttedByCompressor { get; set; }
        [CsvPosition("comp_byp", 53)]
        public int NumberOfBytesBypassedcompressor { get; set; }
        [CsvPosition("comp_rsp", 54)]
        public int NumberOfRequestsCompressed { get; set; }
        [CsvPosition("lastsess", 55)]
        public int NumberOfSecSinceLastSessionAssignment { get; set; }
        [CsvPosition("last_chk", 56)]
        public string LastHealthCheckContent { get; set; }
        [CsvPosition("last_agt", 57)]
        public string LastAgentCheckContent { get; set; }
        [CsvPosition("qtime", 58)]
        public int AverageQueueTimeInMils { get; set; }
        [CsvPosition("ctime", 59)]
        public int AverageConnectTimeInMils { get; set; }
        [CsvPosition("rtime", 60)]
        public int AverageResponseTimeInMils { get; set; }
        [CsvPosition("ttime", 61)]
        public int AverageTotalSessionTime { get; set; }
    }

    public enum HapType
    {
        Frontend = 0,
        Backend = 1,
        Server = 2,
        SocketOrListener = 3
    }
}
