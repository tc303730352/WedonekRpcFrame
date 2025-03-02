using System;
using System.IO;
using WeDonekRpc.Helper;
using WeDonekRpc.WebSocketGateway.Interface;

namespace WeDonekRpc.WebSocketGateway.Model
{

    internal class CurrentSession : ICurrentSession
    {
        private readonly ISession _Session = null;

        public CurrentSession ()
        {
            IApiSocketService service = ApiHandler.ApiService.Value;
            if (service != null)
            {
                this.IsHasValue = true;
                this._Session = service.Session;
            }
        }
        internal CurrentSession (ISession session)
        {
            if (session != null)
            {
                this.IsHasValue = true;
                this._Session = session;
            }
        }

        public string AccreditId => this._Session.AccreditId;

        public string IdentityId => this._Session.IdentityId;

        public bool IsAccredit => this._Session.IsAccredit;

        public bool IsOnline => this._Session.IsOnline;

        public int OfflineTime => this._Session.OfflineTime;

        public Guid SessionId => this._Session.SessionId;

        public bool IsHasValue { get; }

        public string Name => this._Session.Name;

        public void CancelAccredit (string error = null)
        {
            this._Session.CancelAccredit(error);
        }

        public void Send (ErrorException error)
        {
            this._Session.Send(error);
        }

        public void Send (string direct, ErrorException error)
        {
            this._Session.Send(direct, error);
        }

        public void Send (string direct, object data)
        {
            this._Session.Send(direct, data);
        }

        public void Send (Stream stream)
        {
            this._Session.Send(stream);
        }
        public ICurrentSession InitSession (string modular, Guid sessionId)
        {
            IApiModular obj = ApiGateway.GatewayServer.GetModular<IApiModular>(modular);
            if (obj == null)
            {
                throw new ErrorException("gateway.modular.not.find");
            }
            ISession session = obj.GetSession(sessionId);
            return new CurrentSession(session);
        }

        public void SetName (string name)
        {
            this._Session.SetName(name);
        }
    }
}
