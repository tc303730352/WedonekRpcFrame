
using System;

namespace WeDonekRpc.HttpWebSocket.Interface
{
    internal interface ISession : IClientSession
    {
        IWebSocketClient Client { get; }
        Action<ISession> AccreditEvent { get; set; }
        Action<ISession, string> CancelEvent { get; set; }

        /// <summary>
        /// 授权
        /// </summary>
        void Accredit ();
        void Offline ();
    }
}
