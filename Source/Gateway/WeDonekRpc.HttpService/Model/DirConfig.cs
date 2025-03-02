using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using WeDonekRpc.HttpService.Interface;
using WeDonekRpc.Helper;

namespace WeDonekRpc.HttpService.Model
{
    internal class DirConfig
    {
        public string dirPath;
        public string virtualPath;
        public int virtualLen;
        public HttpCacheConfig defCacheSet;
        public ICrossConfig cross;
        public Dictionary<string, HttpCacheConfig> cacheSet;
        private readonly ConcurrentDictionary<string, string> _etagCache = new ConcurrentDictionary<string, string>();
        public HttpCacheSet GetCacheSet (FileInfo file, Uri uri)
        {
            HttpCacheConfig config = this.cacheSet.GetValueOrDefault(file.Extension, this.defCacheSet);
            if (config.CacheType == HttpCacheType.NoStore)
            {
                return new HttpCacheSet
                {
                    CacheType = config.CacheType
                };
            }
            HttpCacheSet cache = new HttpCacheSet
            {
                LastModified = file.LastWriteTime,
                CacheType = config.CacheType,
                SMaxAge = config.SMaxAge,
                MaxAge = config.MaxAge,
                MustRevalidate = config.MustRevalidate
            };
            if (config.EnableEtag)
            {
                cache.Etag = this.ApplyEtag(file, uri);
            }
            return cache;
        }
        public string ApplyEtag (FileInfo file, Uri uri)
        {
            if (this._etagCache.TryGetValue(uri.LocalPath, out string etag))
            {
                return etag;
            }
            etag = Tools.ToBasic64(file.Length + "_" + file.LastAccessTime.ToLong());
            _ = this._etagCache.TryAdd(uri.LocalPath, etag);
            return etag;
        }

        internal string FindEtag (string localPath)
        {
            return this._etagCache.GetValueOrDefault(localPath);
        }
    }
}
