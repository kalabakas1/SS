namespace MPE.SS.Models.ServerInfo
{
    public class Cpu
    {
        public int ClockSpeed { get; set; }
        public int Cores { get; set; }
        public string Description { get; set; }
        public int LogicProcessors { get; set; }
        public string Socket { get; set; }
        public string Status { get; set; }
        public string Manufacturer { get; set; }
        public string Name { get; set; }
        public int Usage { get; set; }
    }
}