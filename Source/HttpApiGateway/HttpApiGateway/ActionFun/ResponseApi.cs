using HttpApiGateway.Interface;
using HttpApiGateway.Model;
using ApiGateway.Model;
using RpcHelper;

namespace HttpApiGateway.ActionFun
{
        internal class ResponseApi : BasicFunc
        {
                public ResponseApi(ApiModel param) : base(param)
                {

                }

                protected override void Invoke(IService service, IApiGateway api, object[] param)
                {
                        IResponse res = (IResponse)this._Method.Invoke(api, param);
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
                public override bool VerificationApi()
                {
                        return this._Params.TrueForAll(a => a.ParamType == FuncParamType.参数);
                }
        }
}
