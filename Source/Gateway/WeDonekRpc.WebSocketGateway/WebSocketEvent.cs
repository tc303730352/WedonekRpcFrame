using WeDonekRpc.Helper;
using WeDonekRpc.HttpWebSocket;
using WeDonekRpc.HttpWebSocket.Interface;
using WeDonekRpc.HttpWebSocket.Model;
using WeDonekRpc.WebSocketGateway.Collect;
using WeDonekRpc.WebSocketGateway.Interface;

namespace WeDonekRpc.WebSocketGateway
{
    internal class WebSocketEvent : SocketEvent
    {
        private readonly IApiModular _Modular = null;

        public WebSocketEvent (IApiModular modular)
        {
            this._Modular = modular;
        }

        public override bool Authorize (RequestBody head)
        {
            if (!ApiPlugInService.Authorize(head))
            {
                return false;
            }
            return this._Modular.Authorize(head);
        }
        public override void AuthorizeComplate (IApiService service)
        {
            this._Modular.AuthorizeComplate(service);
            string text = this._Modular.Config.ResponseTemplate.AuthorizationSuccess();
            service.Response.Write(text);
        }
        public sealed override void Receive (IApiService service)
        {
            if (!ApiPlugInService.RequestInit(service, out string error))
            {
                string text = this._Modular.Config.ResponseTemplate.GetErrorResponse(error);
                service.Response.Write(text);
                return;
            }
            RouteCollect.ExecRoute(service);
        }

        public override void SessionOffline (IClientSession session)
        {
            this._Modular.SessionOffline(session);
        }

        public override void ReplyError (IApiService service, ErrorException error, string source)
        {
            if (source == "AuthorizeComplate")
            {
                string text = this._Modular.Config.ResponseTemplate.AuthorizationFail(error);
                service.Response.Write(text);
                service.CloseCon(30);
            }
            else if (source == "SessionEnd")
            {
                string text = this._Modular.Config.ResponseTemplate.GetErrorResponse(error);
                service.Response.Write(text);
            }
            else if (source == "ReceivePage")
            {
                string text = this._Modular.Config.ResponseTemplate.GetErrorResponse(error);
                service.Response.Write(text);
            }
        }

    }
}
