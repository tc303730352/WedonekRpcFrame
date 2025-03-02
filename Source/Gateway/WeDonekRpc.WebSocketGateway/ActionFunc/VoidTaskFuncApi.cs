using WeDonekRpc.WebSocketGateway.Helper;
using WeDonekRpc.WebSocketGateway.Interface;
using WeDonekRpc.WebSocketGateway.Model;

namespace WeDonekRpc.WebSocketGateway.ActionFunc
{
    internal class VoidTaskFuncApi : BasicFunc
    {
        private readonly ExecTaskAction _Action;
        public VoidTaskFuncApi (ApiModel param) : base(param)
        {
            this._Action = ApiRouteHelper.GetExecTaskAction(param.Method);
        }

        protected override async void Invoke (IWebSocketService service, IApiGateway api, object[] param)
        {
            await this._Action(api, param);
            service.Reply();
        }

    }
}
