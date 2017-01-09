namespace MPE.SS.Models.ServerInfo
{
    public class RAM
    {
        public float TotalGb { get; set; }
        public float FreeGb { get; set; }
        public float UsedGb { get; set; }
        public float PercentageFree { get; set; }
        public float TotalVirtual { get; set; }
        public float FreeVirtual { get; set; }
        public float FreeSpaceInPagingFiles { get; set; }
        public int NumberOfProcesses { get; set; }
        public string NumberOfUsers { get; set; }
    }
}