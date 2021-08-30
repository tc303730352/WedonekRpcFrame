using System;
using System.IO;

using RpcHelper;

using WebSocketGateway.Interface;

namespace WebSocketGateway.Model
{

        internal class CurrentSession : ICurrentSession
        {
                private readonly ISession _Session = null;

                public CurrentSession()
                {
                        IApiSocketService service = ApiHandler.ApiService;
                        if (service != null)
                        {
                                this.IsHasValue = true;
                                this._Session = service.Session;
                        }
                }
                public CurrentSession(ISession session)
                {
                        if (session != null)
                        {
                                this.IsHasValue = true;
                                this._Session = session;
                        }
                }

                public string AccreditId => _Session.AccreditId;

                public string IdentityId => _Session.IdentityId;

                public bool IsAccredit => _Session.IsAccredit;

                public bool IsOnline => _Session.IsOnline;

                public int OfflineTime => _Session.OfflineTime;

                public Guid SessionId => _Session.SessionId;

                public bool IsHasValue { get; }

                public string Name => _Session.Name;

                public void CancelAccredit(string error = null)
                {
                        _Session.CancelAccredit(error);
                }

                public void Send(ErrorException error)
                {
                        _Session.Send(error);
                }

                public void Send(string direct, ErrorException error)
                {
                        _Session.Send(direct, error);
                }

                public void Send(string direct, object data)
                {
                        _Session.Send(direct, data);
                }

                public void Send(Stream stream)
                {
                        _Session.Send(stream);
                }
                public ICurrentSession InitSession(string modular, Guid sessionId)
                {
                        IApiModular obj = ApiGateway.GatewayServer.GetModular<IApiModular>(modular);
                        if (obj == null)
                        {
                                throw new ErrorException("gateway.modular.not.find");
                        }
                        ISession session = obj.GetSession(sessionId);
                        return new CurrentSession(session);
                }

                public void SetName(string name)
                {
                        _Session.SetName(name);
                }
        }
}
