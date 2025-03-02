using WeDonekRpc.ApiGateway.Model;

using WeDonekRpc.HttpApiGateway.Helper;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpApiGateway.Model;

namespace WeDonekRpc.HttpApiGateway.ActionFun
{
    internal class VoidFuncApi : BasicFunc
    {
        private readonly ExecAction _Action;
        public VoidFuncApi(ApiModel param) : base(param)
        {
            this._Action = ApiRouteHelper.GetExecAction(param.Method);
        }

        protected override void Invoke(IService service, IApiGateway api, object[] param)
        {
            this._Action(api, param);
            service.Reply();
        }

    }
}
