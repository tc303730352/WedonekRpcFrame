using System;
using WeDonekRpc.HttpWebSocket.Model;
using WeDonekRpc.Helper;

namespace WeDonekRpc.HttpWebSocket.Interface
{
    internal interface ISocketServer
    {
        Guid Id { get; }
        IWebSocketConfig Config { get; }

        IClientSessionCollect Session { get; }

        void Sync ();

        bool IsSSL { get; }

        void CheckSessionState ( IWebSocketClient client );

        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        bool Authorize ( RequestBody request );

        void AuthorizeComplate ( IWebSocketClient client );

        void RemoveClient ( Guid clientId );
        void Close ();
        void ReceiveData ( IWebSocketClient client, PageType type, byte[] content );
        void ReplyError ( IWebSocketClient client, ErrorException error, string source );
        /// <summary>
        /// 检查客户端是否在线
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        bool CheckIsOnline ( Guid clientId );
        void CloseCon ( Guid clientId, int time );
        void CloseCon ( Guid clientId );
    }
}