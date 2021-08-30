using System;
using System.Net;

namespace RpcHelper
{
        public struct HttpResponseRes
        {
                public HttpStatusCode StatusCode
                {
                        get;
                        set;
                }
                public string Content
                {
                        get;
                        set;
                }
                public Exception Error
                {
                        get;
                        set;
                }
                public KeValue[] Cookies { get; set; }
        }
        public struct KeValue
        {
                public string Name
                {
                        get;
                        set;
                }
                public string Value
                {
                        get;
                        set;
                }
        }
}
