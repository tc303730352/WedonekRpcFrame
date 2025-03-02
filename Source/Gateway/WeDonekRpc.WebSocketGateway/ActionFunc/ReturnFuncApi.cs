using WeDonekRpc.ApiGateway.Model;
using WeDonekRpc.WebSocketGateway.Helper;
using WeDonekRpc.WebSocketGateway.Interface;
using WeDonekRpc.WebSocketGateway.Model;

namespace WeDonekRpc.WebSocketGateway.ActionFunc
{
    internal class ReturnFuncApi : BasicFunc
    {
        private readonly ExecFunc _Action;
        public ReturnFuncApi (ApiModel param) : base(param)
        {
            this._Action = ApiRouteHelper.GetExecFunc(param.Method);
        }

        protected override void Invoke (IWebSocketService service, IApiGateway api, object[] param)
        {
            object res = this._Action(api, param);
            service.Reply(res);
        }

        protected override void InitApiBody (IApiRoute route, ApiFuncBody body)
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
