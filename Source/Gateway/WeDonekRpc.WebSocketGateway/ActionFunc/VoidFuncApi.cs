using WeDonekRpc.WebSocketGateway.Helper;
using WeDonekRpc.WebSocketGateway.Interface;
using WeDonekRpc.WebSocketGateway.Model;

namespace WeDonekRpc.WebSocketGateway.ActionFunc
{
    internal class VoidFuncApi : BasicFunc
    {
        private readonly ExecAction _Action;
        public VoidFuncApi (ApiModel param) : base(param)
        {
            this._Action = ApiRouteHelper.GetExecAction(param.Method);
        }

        protected override void Invoke (IWebSocketService service, IApiGateway api, object[] param)
        {
            this._Action(api, param);
            service.Reply();
        }

    }
}
