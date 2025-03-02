using System;

using WeDonekRpc.HttpWebSocket.Model;

namespace WeDonekRpc.HttpWebSocket.Interface
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
                dynamic Format(Type dataType);
                object FormatObject(Type dataType);
        }
}