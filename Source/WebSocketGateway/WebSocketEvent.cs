
using System;

using ApiGateway.Interface;

using HttpWebSocket;
using HttpWebSocket.Interface;
using HttpWebSocket.Model;

using RpcHelper;

using WebSocketGateway.Collect;
using WebSocketGateway.Interface;

namespace WebSocketGateway
{
        internal class WebSocketEvent : SocketEvent
        {
                private readonly IApiModular _Modular = null;

                public WebSocketEvent(IApiModular modular)
                {
                        this._Modular = modular;
                }

                public override bool Authorize(RequestBody head)
                {
                        return this._Modular.Authorize(head);
                }
                public override void AuthorizeComplate(IApiService service)
                {
                        this._Modular.AuthorizeComplate(service);
                        string text = this._Modular.Config.ResponseTemplate.AuthorizationSuccess();
                        service.Response.Write(text);
                }
                public override void CheckSession(IClientSession session)
                {
                        if (session.IsAccredit)
                        {
                                this._Modular.Config.CheckAccredit(session.AccreditId);
                        }
                }
                public sealed override void Receive(IApiService service)
                {
                        RouteCollect.ExecRoute(service);
                }

                public override void SessionOffline(IClientSession session)
                {
                        this._Modular.SessionOffline(session);
                }

                public override void ReplyError(IApiService service, ErrorException error,string source)
                {
                        if (source == "AuthorizeComplate")
                        {
                                string text = this._Modular.Config.ResponseTemplate.AuthorizationFail(error);
                                service.Response.Write(text);
                                service.CloseCon(30);
                        }
                        else if(source == "SessionEnd")
                        {
                                string text = this._Modular.Config.ResponseTemplate.GetErrorResponse(error);
                                service.Response.Write(text);
                        }
                        else if(source == "ReceivePage")
                        {
                                string text = this._Modular.Config.ResponseTemplate.GetErrorResponse(error);
                                service.Response.Write(text);
                        }
                }

        }
}
