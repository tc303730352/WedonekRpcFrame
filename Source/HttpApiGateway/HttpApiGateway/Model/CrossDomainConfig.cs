using System;

using RpcHelper;

namespace HttpApiGateway.Model
{
        public class CrossDomainConfig
        {
                /// <summary>
                /// 允许跨域
                /// </summary>
                public bool AllowCredentials
                {
                        get;
                        set;
                } = true;
                /// <summary>
                /// 跨域限定允许访问来源
                /// </summary>
                public string[] AllowUrlReferrer
                {
                        get;
                        set;
                }
                /// <summary>
                /// 跨域限定头部
                /// </summary>
                public string AllowHead { get; set; } = "*";
                /// <summary>
                /// 有效时间
                /// </summary>
                public string MaxAge
                {
                        get;
                        set;
                } = "3600";

                /// <summary>
                /// 允许的请求方式 ,号分隔
                /// </summary>
                public string Method
                {
                        get;
                        set;
                } = "POST,GET,OPTIONS";
                public bool CheckUrlReferrer(Uri referrer)
                {
                        if (!this.AllowCredentials)
                        {
                                return false;
                        }
                        return this.AllowUrlReferrer == null || this.AllowUrlReferrer.IsExists(a => a == referrer.Authority || referrer.Authority.EndsWith(a));
                }
        }
}
