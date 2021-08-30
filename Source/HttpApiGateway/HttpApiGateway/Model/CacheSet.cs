using System;

namespace HttpApiGateway.Model
{
        public class CacheSet
        {
                /// <summary>
                /// 是否禁用缓存
                /// </summary>
                public bool IsNoCache
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
                public int CacheTime { get; set; }

                /// <summary>
                /// 最后更新时间
                /// </summary>
                public DateTime ToUpdateTime { get; set; }
        }
}
