using WeDonekRpc.ApiGateway.Model;
using WeDonekRpc.HttpApiGateway.Helper;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpApiGateway.Model;

namespace WeDonekRpc.HttpApiGateway.ActionFun
{
    internal class ReturnFuncApi : BasicFunc
    {
        private readonly ExecFunc _Action;
        public ReturnFuncApi(ApiModel param) : base(param)
        {
            this._Action = ApiRouteHelper.GetExecFunc(param.Method);
        }

        protected override void Invoke(IService service, IApiGateway api, object[] param)
        {
            object res = this._Action(api, param);
            service.Reply(res);
        }

        protected override void InitApiBody(IApiRoute route, ApiFuncBody body)
        {
            body.Results = new ResultBody[]
            {
                new ResultBody
                {
                    ResultType = this._Method.ReturnType,
                    ParamName = "returns",
                    AttrName = "Returns"
                }
            };
        }
    }
}
