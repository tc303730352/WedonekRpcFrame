using System.Collections.Generic;

using ApiGateway.Model;

using RpcHelper;

using WebSocketGateway.Interface;
using WebSocketGateway.Model;

namespace WebSocketGateway.Route
{

        internal class OutFuncApi : BasicFunc
        {
                private readonly int[] _Outs = null;

                private readonly int _ErrorIndex = 0;
                public OutFuncApi(ApiModel param) : base(param)
                {
                        this._Outs = this._Params.ConvertIndex(a => a.ParamType == FuncParamType.返回值);
                        this._ErrorIndex = this._Params.FindIndex(a => a.ParamType == FuncParamType.错误信息);
                }
                private object _GetReturns(object[] param)
                {
                        if (this._Outs == null)
                        {
                                return null;
                        }
                        else if (this._Outs.Length == 1)
                        {
                                return param[this._Outs[0]];
                        }
                        Dictionary<string, object> obj = new Dictionary<string, object>();
                        this._Outs.ForEach(a =>
                        {
                                FuncParam p = this._Params[a];
                                obj.Add(p.AttrName, param[a]);
                        });
                        return obj;
                }
                protected override void Invoke(IWebSocketService service, IApiGateway api, object[] param)
                {
                        if (!(bool)this._Method.Invoke(api, param))
                        {
                                service.ReplyError((string)param[this._ErrorIndex]);
                        }
                        else
                        {
                                object res = this._GetReturns(param);
                                if (this._IsPagingFun)
                                {
                                        service.Reply(res, param[this._CountIndex]);
                                }
                                else if (res == null)
                                {
                                        service.Reply();
                                }
                                else
                                {
                                        service.Reply(res);
                                }
                        }
                }

                protected override void InitApiBody(IApiRoute route, ApiFuncBody body)
                {
                        if (!this._Outs.IsNull())
                        {
                                body.Results = this._Outs.ConvertAll(a =>
                                {
                                        FuncParam func = this._Params[a];
                                        return new ResultBody
                                        {
                                                AttrName = func.AttrName,
                                                ParamName = func.Name,
                                                ResultType = func.DataType
                                        };
                                });
                        }
                }
                public override bool VerificationApi()
                {
                        if (this._IsPagingFun && this._Params.FindIndex(b => b.ParamType == FuncParamType.返回值 && (b.DataType.IsArray || b.DataType.IsGenericType)) == -1)
                        {
                                return false;
                        }
                        return true;
                }

        }
}
