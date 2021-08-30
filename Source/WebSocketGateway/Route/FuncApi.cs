
using ApiGateway.Interface;
using ApiGateway.Model;

using RpcHelper;

using WebSocketGateway.Helper;
using WebSocketGateway.Interface;
using WebSocketGateway.Model;

namespace WebSocketGateway.Route
{
        internal class FuncApi : BasicFunc
        {
                private readonly int _ErrorIndex = -1;

                private readonly int[] _Outs = null;
                public FuncApi(ApiModel param) : base(param)
                {
                        this._Outs = this._Params.ConvertIndex(a => a.ParamType == FuncParamType.返回值);
                        this._ErrorIndex = this._Params.FindIndex(a => a.ParamType == FuncParamType.错误信息);
                }
                protected override void InitApiBody(IApiRoute route, ApiFuncBody body)
                {
                        ResultBody[] rets = this._Params.Convert(a => a.ParamType == FuncParamType.返回值, a => new ResultBody
                        {
                                ParamName = a.Name,
                                ResultType = a.DataType,
                                AttrName = a.AttrName
                        });
                        if (this._Method.ReturnType == PublicDataDic.VoidType)
                        {
                                body.Results = rets;
                        }
                        else
                        {
                                body.Results = rets.Add(new ResultBody
                                {
                                        ResultType = this._Method.ReturnType,
                                        ParamName = "returns",
                                        AttrName = "Returns"
                                });
                        }
                }
                protected override void Invoke(IWebSocketService service, IApiGateway api, object[] param)
                {
                        object result = this._Method.Invoke(api, param);
                        result = FuncHelper.GetReturns(result, this._Outs, param, this._Params);
                        if (this._IsPagingFun)
                        {
                                service.Reply(result, param[this._CountIndex]);
                        }
                        else if (result != null)
                        {
                                service.Reply(result);
                        }
                        else if (this._ErrorIndex == -1)
                        {
                                service.Reply();
                        }
                        else
                        {
                                string error = (string)param[this._ErrorIndex];
                                if (error != null)
                                {
                                        service.ReplyError(error);
                                        return;
                                }
                                service.Reply();
                        }
                }
        }
}
