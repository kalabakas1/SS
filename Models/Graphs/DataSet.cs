using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MPE.SS.Models.Graphs
{
    public class DataSet
    {
        public DataSet()
        {
            Color = new Color(153, 255, 51);
        }
        public string Label { get; set; }
        public List<Point> Points { get; set; }
        public Color Color { get; set; }
        public bool DontFill { get; set; }

        public string RenderLabels()
        {
            return JsonConvert.SerializeObject(Points.Select(x => x.Label));
        }

        public string RenderPoints()
        {
            return JsonConvert.SerializeObject(Points.Select(x => new
            {
                x = x.Label,
                y = x.Value?.ToString()
            }));
        }
    }
}
