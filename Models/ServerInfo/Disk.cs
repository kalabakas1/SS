namespace MPE.SS.Models.ServerInfo
{
    public class Disk
    {
        public string Letter { get; set; }
        public string FreeSpaceMb { get; set; }
        public string TotalSpaceMb { get; set; }
        public string UsedSpaceMb { get; set; }
    }
}