﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPE.SS.Interfaces
{
    internal interface ICsvService<T>
    {
        T ParseLine(string line);
    }
}
