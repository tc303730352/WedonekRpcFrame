using System;
using WeDonekRpc.HttpWebSocket.Interface;

namespace WeDonekRpc.HttpWebSocket
{
    internal class BasicServer : IService
    {
        private readonly ISocketServer _Server = null;
        public BasicServer (ISocketServer server)
        {
            this._Server = server;
        }
        public Guid Id => this._Server.Id;

        public IWebSocketConfig Config => this._Server.Config;

        public void CancelAccredit (string accreditId, string error)
        {
            this._Server.Session.Cancel(accreditId, error);
        }

        public void Close ()
        {
            this._Server.Close();
        }

        public IClientSession[] FindSession (string accreditId)
        {
            return this._Server.Session.FindSession(accreditId);
        }
        public IClientSession[] GetSession (Guid[] sessionId)
        {
            return this._Server.Session.GetSession(sessionId);
        }
        public IClientSession[] FindSession (Func<ISessionBody, bool> find)
        {
            return this._Server.Session.FindSession(find);
        }
        public IClientSession GetSession (Guid sessionId)
        {
            return this._Server.Session.GetSession(sessionId);
        }

        public IClientSession FindOnlineSession (string accreditId, string name)
        {
            return this._Server.Session.FindOnlineSession(accreditId, name);
        }
    }
}
