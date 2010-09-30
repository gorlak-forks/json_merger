using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace json_merge
{
    public static class DictionaryDefaultExtension
    {
        public static V GetValueOrDefault<K, V>(this Dictionary<K, V> dic, K key, V def)
        {
            V ret;
            bool found = dic.TryGetValue(key, out ret);
            if (found) { return ret; }
            return def;
        }
    }

    public static class ObjectCloneExtension
    {
        public static T DeepClone<T>(this T a)
        {
            using (Stream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, a);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
