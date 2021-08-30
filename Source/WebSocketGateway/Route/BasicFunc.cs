using System;
using System.Reflection;

using ApiGateway.Model;

using RpcHelper;

using WebSocketGateway.Collect;
using WebSocketGateway.Helper;
using WebSocketGateway.Interface;
using WebSocketGateway.Model;

namespace WebSocketGateway.Route
{
        internal class BasicFunc : ISocketApi
        {
                public string LocalPath { get; }
                private readonly bool _IsForm = false;

                protected readonly MethodInfo _Method;

                private readonly Type _SourceType = null;

                public string ApiName { get; }

                private readonly string _ApiShow;

                protected readonly FuncParam[] _Params;

                protected readonly int _CountIndex = -1;

                protected readonly bool _IsPagingFun = false;
                public BasicFunc(ApiModel param)
                {
                        this.LocalPath = param.LocalPath;
                        this._SourceType = param.Type;
                        this.ApiName = param.Method.Name;
                        this._Method = param.Method;
                        this._ApiShow = string.Format("Name:{0}\r\nMethod:{1}\r\nSource:{2}", this.ApiName, this._Method.Name, param.Type.FullName);
                        this._Params = FuncHelper.InitMethod(this._Method);
                        this._CountIndex = this._Params.FindIndex(a => a.ParamType == FuncParamType.数据总数);
                        this._IsForm = this._Params.Count(a => a.ParamType == FuncParamType.参数) > 1;
                        this._IsPagingFun = this._CountIndex != -1;
                }

                private void _InvokeMethod(IWebSocketService service, object[] param)
                {
                        IApiGateway api = UnityCollect.GetGateway(this._SourceType);
                        try
                        {
                                this.Invoke(service, api, param);
                        }
                        catch (Exception e)
                        {
                                ErrorException error = ErrorException.FormatError(e);
                                GatewayLog.AddErrorLog(error, this._ApiShow);
                                service.ReplyError(error.ToString());
                        }
                }
                protected virtual void Invoke(IWebSocketService service, IApiGateway api, object[] param)
                {

                }
                public void ExecApi(IWebSocketService service)
                {
                        if (!FuncHelper.InitMethodParam(this._Params, this._IsForm, this._Method, service, out object[] param, out string error))
                        {
                                service.ReplyError(error);
                        }
                        else
                        {
                                this._InvokeMethod(service, param);
                        }
                }

                public void RegApi(IApiRoute route)
                {
                        ApiFuncBody param = new ApiFuncBody
                        {
                                PostParam = this._Params.Convert(a => a.ParamType == FuncParamType.参数, (a, i) => new ApiPostParam
                                {
                                        Name = a.Name,
                                        PostType = a.DataType,
                                        ParamIndex = i
                                }),
                                GatewayType = ApiGateway.GatewayType.WebSocket,
                                ApiType = ApiGateway.ApiType.API接口,
                                IsPaging = _IsPagingFun,
                                Source = _SourceType,
                                IsAccredit = route.IsAccredit,
                                Method = _Method,
                                Prower = route.Prower,
                                ApiUri = route.LocalPath
                        };
                        this.InitApiBody(route, param);
                        GatewayService.RegApi(param);
                }
                protected virtual void InitApiBody(IApiRoute route, ApiFuncBody body)
                {

                }

                public virtual bool VerificationApi()
                {
                        if (this._IsPagingFun && !this._Method.ReturnType.IsArray && !this._Method.ReturnType.IsGenericType)
                        {
                                return false;
                        }
                        return true;
                }
        }
}
