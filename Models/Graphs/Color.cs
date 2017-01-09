using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPE.SS.Models.Graphs
{
    public class Color
    {
        private const int MaxRgb = 255;
        public Color(int red, int green, int blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public Color()
        {
            Red = 153;
            Green = 255;
            Blue = 51;
        }

        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }

        public override string ToString()
        {
            return string.Format("rgba({0},{1},{2},0.4)", Red, Green, Blue);
        }

        public Color Negative()
        {
            return new Color
            {
                Red = MaxRgb - Red,
                Green = MaxRgb - Green,
                Blue = MaxRgb - Blue
            };
        }
    }
}
