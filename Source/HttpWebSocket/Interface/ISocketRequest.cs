using System;

using HttpWebSocket.Model;

namespace HttpWebSocket.Interface
{
        public interface ISocketRequest
        {
                /// <summary>
                /// 内容
                /// </summary>
                byte[] Content { get; }
                /// <summary>
                /// 请求头
                /// </summary>
                RequestBody Head { get; }
               

                T FormatObject<T>() where T : class;
                string FormatString();
                object Format(Type dataType);
                object FormatObject(Type dataType);
        }
}