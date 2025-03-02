using WeDonekRpc.ApiGateway.Model;
using WeDonekRpc.HttpApiGateway.Helper;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpApiGateway.Model;

namespace WeDonekRpc.HttpApiGateway.ActionFun
{
    internal class ResponseApi : BasicFunc
    {
        private ExecResponse _Action;
        public ResponseApi(ApiModel param) : base(param)
        {
            _Action = ApiRouteHelper.GetExecResponse(param.Method);
        }

        protected override void Invoke(IService service, IApiGateway api, object[] param)
        {
            IResponse res = this._Action(api, param);
            if (res == null)
            {
                service.ReplyError("http.404");
                return;
            }
            service.Reply(res);
        }


        protected override void InitApiBody(IApiRoute route, ApiFuncBody body)
        {
            body.Results = new ResultBody[]
            {
                new ResultBody
                {
                        ResultType=this._Method.ReturnType,
                        AttrName="Returns",
                        ParamName="returns"
                }
            };
        }
    }
}
