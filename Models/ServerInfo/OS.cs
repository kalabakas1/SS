namespace MPE.SS.Models.ServerInfo
{
    public class OS
    {
        public Boottime BootTime { get; set; }
        public string SystemDrive { get; set; }
        public string SystemDevice { get; set; }
        public int Language { get; set; }
        public string Version { get; set; }
        public string WindowsDirectory { get; set; }
        public string Name { get; set; }
        public Installdate InstallDate { get; set; }
        public string ServicePack { get; set; }
    }
}