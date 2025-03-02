using System;

namespace WeDonekRpc.HttpService.Model
{
    public enum HttpCacheType
    {
        Private = 0,
        Public = 1,
        NoCache = 2,
        NoStore = 3
    }
    public class HttpCacheSet
    {
        public HttpCacheSet()
        {

        }
        public HttpCacheSet(HttpCacheType cacheType)
        {
            this.CacheType= cacheType;
        }
        /// <summary>
        /// 缓存类型
        /// </summary>
        public HttpCacheType CacheType
        {
            get;
            set;
        }
        /// <summary>
        /// 缓存的Etag
        /// </summary>
        public string Etag { get; set; }
        /// <summary>
        /// 缓存时间
        /// </summary>
        public int? MaxAge { get; set; }
        /// <summary>
        /// 代理服务器缓存时间
        /// </summary>
        public int? SMaxAge { get; set; }
        /// <summary>
        /// 告诉浏览器、缓存服务器，本地副本过期前，可以使用本地副本；本地副本一旦过期，必须去源服务器进行有效性校验。
        /// </summary>
        public bool MustRevalidate { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime? LastModified { get; set; }
    }
}
