using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPE.SS.Logic.Extensions
{
    internal static class DictionaryExtensions
    {
        public static void AddOrUpdate<T, TK>(this Dictionary<T, TK> dict, T key, TK value)
        {
            if (dict.ContainsKey(key))
                dict[key] = value;
            else
                dict.Add(key, value);
        }
    }
}
