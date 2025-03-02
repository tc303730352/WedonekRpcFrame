using System;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpWebSocket.Interface;
using WeDonekRpc.WebSocketGateway.Interface;

namespace WeDonekRpc.WebSocketGateway.Model
{
    internal class CurrentModular : ICurrentModular
    {
        private readonly IApiModular _Modular = null;
        public CurrentModular ()
        {
            IApiSocketService service = ApiHandler.ApiService.Value;
            if (service != null)
            {
                this.IsHasValue = true;
                this._Modular = ApiGateway.GatewayServer.GetModular<IApiModular>(service.ServiceName);
            }
        }
        internal CurrentModular (IApiModular modular)
        {
            this.IsHasValue = true;
            this._Modular = modular;
        }
        public bool IsHasValue
        {
            get;
        }
        public string ServerName => this._Modular.ServiceName;

        public ISession GetSession (Guid sessionId)
        {
            return this._Modular.GetSession(sessionId);
        }
        public ISession[] GetSession (Guid[] sessionId)
        {
            return this._Modular.GetSession(sessionId);
        }
        public void CancelAccredit (string accreditId, string error)
        {
            this._Modular.CancelAccredit(accreditId, error);
        }
        public ISession[] FindSession (string accreditId)
        {
            return this._Modular.FindSession(accreditId);
        }

        public ICurrentModular Init (string name)
        {
            IApiModular modular = ApiGateway.GatewayServer.GetModular<IApiModular>(name);
            if (modular == null)
            {
                throw new ErrorException("gateway.modular.not.find");
            }
            return new CurrentModular(modular);
        }
        public IBatchSend BatchSend (Func<ISessionBody, bool> find)
        {
            return this._Modular.BatchSend(find);
        }
        public IBatchSend BatchSend (Guid[] sessionId)
        {
            return this._Modular.BatchSend(sessionId);
        }
        public ISession[] FindSession (Func<ISessionBody, bool> find)
        {
            return this._Modular.FindSession(find);
        }
        public ISession FindOnlineSession (string accreditId, string name)
        {
            return this._Modular.FindOnlineSession(accreditId, name);
        }
    }
}
