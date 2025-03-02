using System;

namespace WeDonekRpc.HttpWebSocket.Interface
{
    internal interface IClientSessionCollect
    {
        void ClearSession ();
        ISession CreateSession (IWebSocketClient client);
        ISession GetSession (Guid sessionId);
        ISession Offline (Guid sessionId);
        void Cancel (string accreditId, string error);
        IClientSession[] FindSession (string accreditId);
        IClientSession[] FindSession (Func<ISessionBody, bool> find);
        IClientSession[] GetSession (Guid[] sessionId);
        IClientSession FindOnlineSession (string accreditId, string name);
    }
}