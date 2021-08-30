using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;

namespace RpcHelper.Json
{
        internal class InterfaceCacheHelper
        {
                private static readonly ConcurrentDictionary<string, InterfaceCache> _CacheList = new ConcurrentDictionary<string, InterfaceCache>();

                public static bool CanConvert(Type type)
                {
                        if (_CacheList.TryGetValue(type.FullName, out InterfaceCache cache))
                        {
                                return cache.IsCanConvert;
                        }
                        cache = new InterfaceCache(type);
                        cache.Init();
                        _CacheList.TryAdd(type.FullName, cache);
                        return cache.IsCanConvert;
                }

                internal static object Read(JsonReader reader,Type type, JsonSerializer serializer)
                {
                        return _CacheList[type.FullName].Read(reader, serializer);
                }
        }
}
