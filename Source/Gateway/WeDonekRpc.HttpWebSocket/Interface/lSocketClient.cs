using System;
using System.Net;

namespace WeDonekRpc.HttpWebSocket.Interface
{
        internal interface lSocketClient : IDisposable
        {
                bool IsAuthorize { get; }
                IPEndPoint RemoteIp { get; }
                bool IsCon { get; }

                void InitSocket();
                void Authorize();
                void ReceiveComplate();

                bool Send(byte[] datas);
                bool Close(int time);
                void Close();
        }
}