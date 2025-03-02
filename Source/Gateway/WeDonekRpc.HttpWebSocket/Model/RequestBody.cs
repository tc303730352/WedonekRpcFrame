using System;

namespace WeDonekRpc.HttpWebSocket.Model
{
        /// <summary>
        /// 请求体
        /// </summary>
        public class RequestBody
        {
                public RequestBody(HttpHead head, string ip)
                {
                        this.RemoteIp = ip;
                        this.Path = head.Path;
                        this.HttpVer = head.HttpVer;
                        this.Origin = head.Origin;
                        this.UserAgent = head.UserAgent;
                        this.WebSocketVer = head.WebSocketVer;
                }
                /// <summary>
                /// 请求路径
                /// </summary>
                public string Path
                {
                        get;
                }
                /// <summary>
                /// 远端IP
                /// </summary>
                public string RemoteIp { get; }

                /// <summary>
                /// HTTP版本号
                /// </summary>
                public string HttpVer { get; }
                /// <summary>
                /// WebSocket版本
                /// </summary>
                public string WebSocketVer { get; }
                /// <summary>
                /// 来源URI
                /// </summary>
                public string Origin { get; }

                /// <summary>
                /// 用户头
                /// </summary>
                public string UserAgent { get; }
        }
}
