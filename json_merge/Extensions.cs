using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace json_merge
{
    public static class Extensions
    {
        public static V GetValueOrDefault<K, V>(this Dictionary<K, V> dic, K key, V def)
        {
            V ret;
            bool found = dic.TryGetValue(key, out ret);
            if (found) { return ret; }
            return def;
        }
    }
}
