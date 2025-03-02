using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Config;

namespace WeDonekRpc.CacheClient.Config
{
    internal enum CacheMode
    {
        随机 = 0,
        固定 = 1,
        永久 = 2,
        程序为准 = 3
    }
    internal class CacheTimeConfig
    {
        private static char _SpaceChar = '_';
        private static _CahceTime _Def;
        private static IConfigSection _Config;
        private static readonly ConcurrentDictionary<string, _CahceTime> _cache = new ConcurrentDictionary<string, _CahceTime>();
        private static bool _IsEnable = false;
        private class _CahceTime
        {
            public CacheMode cacheMode;
            public TimeSpan? time;
            public bool isPriority;
            public int? randomMin;
            public int? randomMax;
            public void Check (_CahceTime def)
            {
                if (this.cacheMode == CacheMode.固定 && !this.time.HasValue)
                {
                    this.time = def.time;
                }
                else if (this.cacheMode == CacheMode.随机)
                {
                    if (!this.randomMin.HasValue)
                    {
                        this.randomMin = def.randomMin.Value;
                    }
                    if (!this.randomMax.HasValue)
                    {
                        this.randomMax = def.randomMax.Value;
                    }
                }
            }
            public TimeSpan? GetTimeSpan (TimeSpan? time)
            {
                if (this.cacheMode == CacheMode.程序为准)
                {
                    return time;
                }
                else if (time.HasValue && !this.isPriority)
                {
                    return time;
                }
                else if (this.cacheMode == CacheMode.永久)
                {
                    return null;
                }
                else if (this.cacheMode == CacheMode.固定)
                {
                    return time;
                }
                else
                {
                    int sec = Tools.GetRandom(this.randomMin.Value, this.randomMax.Value);
                    return TimeSpan.FromSeconds(sec);
                }
            }
        }
        private static void _LoadCacheTime (string name)
        {
            _CahceTime template = new _CahceTime
            {
                cacheMode = CacheMode.随机,
                isPriority = false,
                time = TimeSpan.FromDays(1),
                randomMin = 3600,
                randomMax = 43200
            };
            if (name.IsNull() || name == "def")
            {
                _CahceTime def = _Config.GetValue<_CahceTime>("def", template);
                def.Check(template);
                _Def = def;
            }
            if (name.IsNull() || name.StartsWith("items:"))
            {
                Dictionary<string, _CahceTime> caches = _Config.GetValue<Dictionary<string, _CahceTime>>("items");
                if (caches != null && caches.Count > 0)
                {
                    string[] keys = _cache.Keys.ToArray();
                    caches.ForEach((key, val) =>
                    {
                        val.Check(template);
                        _cache.AddOrSet(key, val);
                    });
                    if (keys.Length > 0)
                    {
                        keys = keys.FindAll(a => !caches.ContainsKey(a));
                        if (keys.Length > 0)
                        {
                            keys.ForEach(a => _cache.Remove(a, out _));
                        }
                    }
                }
            }
        }

        public static void Init ()
        {
            _Config = LocalConfig.Local.GetSection("rpc:cacheTime");
            _Config.AddRefreshEvent(_Local_RefreshEvent);
        }

        private static void _Local_RefreshEvent (IConfigSection config, string name)
        {
            _IsEnable = config.GetValue<bool>("IsEnable", false);
            _SpaceChar = config.GetValue<char>("Space", '_');
            _LoadCacheTime(name);
        }

        public static TimeSpan? FormatCacheTime (string key, TimeSpan? time)
        {
            if (!_IsEnable)
            {
                return time;
            }
            int index = key.IndexOf(_SpaceChar);
            if (index == -1)
            {
                return time;
            }
            key = key.Substring(0, index);
            if (_cache.TryGetValue(key, out _CahceTime cache))
            {
                return cache.GetTimeSpan(time);
            }
            return _Def.GetTimeSpan(time);
        }

        internal static void Close ()
        {
            _Config.RemoveRefreshEvent(_Local_RefreshEvent);
        }
    }
}
