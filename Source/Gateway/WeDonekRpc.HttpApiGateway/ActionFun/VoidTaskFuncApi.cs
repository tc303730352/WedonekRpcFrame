using WeDonekRpc.HttpApiGateway.Helper;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpApiGateway.Model;

namespace WeDonekRpc.HttpApiGateway.ActionFun
{
    internal class VoidTaskFuncApi : BasicFunc
    {
        private readonly ExecTaskAction _Action;
        public VoidTaskFuncApi (ApiModel param) : base(param)
        {
            this._Action = ApiRouteHelper.GetExecTaskAction(param.Method);
        }

        protected override async void Invoke (IService service, IApiGateway api, object[] param)
        {
            await this._Action(api, param);
            service.Reply();
        }

    }
}
