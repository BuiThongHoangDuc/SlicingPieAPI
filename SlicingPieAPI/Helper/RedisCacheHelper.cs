using SlicingPieAPI.DTOs;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace SlicingPieAPI.Helper
{
    public static class RedisCacheHelper
    {
        public static T Get<T>(string cacheKey, IDatabase cache)
        {
            return Deserialize<T>(cache.StringGet(cacheKey));
        }

        public static object Get(string cacheKey, IDatabase cache)
        {
            return Deserialize<object>(cache.StringGet(cacheKey));
        }

        public static void Set(string cacheKey, object cacheValue, IDatabase cache)
        {
            cache.StringSet(cacheKey, Serialize(cacheValue));
        }

        private static byte[] Serialize(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            BinaryFormatter objBinaryFormatter = new BinaryFormatter();
            using (MemoryStream objMemoryStream = new MemoryStream())
            {
                objBinaryFormatter.Serialize(objMemoryStream, obj);
                byte[] objDataAsByte = objMemoryStream.ToArray();
                return objDataAsByte;
            }
        }

        private static T Deserialize<T>(byte[] bytes)
        {
            BinaryFormatter objBinaryFormatter = new BinaryFormatter();
            if (bytes == null)
                return default(T);

            using (MemoryStream objMemoryStream = new MemoryStream(bytes))
            {
                T result = (T)objBinaryFormatter.Deserialize(objMemoryStream);
                return result;
            }
        }
    }
}
