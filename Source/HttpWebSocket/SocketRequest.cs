using System;

using HttpWebSocket.Config;
using HttpWebSocket.Interface;
using HttpWebSocket.Model;

using RpcHelper;

namespace HttpWebSocket
{
        public class SocketRequest : ISocketRequest
        {
                public SocketRequest(RequestBody head, byte[] content)
                {
                        this.Head = head;
                        this.Content = content;
                }

                public byte[] Content { get; }

                public RequestBody Head { get; }

                public object Format(Type dataType)
                {
                        string str = this.FormatString();
                        return str.ToObject(dataType);
                }

                public T FormatObject<T>() where T : class
                {
                        string str = this.FormatString();
                        if (str == null)
                        {
                                return null;
                        }
                        return str.Json<T>();
                }
                public object FormatObject(Type type)
                {
                        string str = this.FormatString();
                        if (str == null)
                        {
                                return null;
                        }
                        return str.Json(type);
                }
                public string FormatString()
                {
                        return PublicConfig.ResponseEncoding.GetString(this.Content);
                }
        }
}
