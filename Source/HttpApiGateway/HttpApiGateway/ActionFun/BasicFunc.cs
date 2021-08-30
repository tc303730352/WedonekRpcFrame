using System;
using System.Reflection;

using ApiGateway.Model;

using HttpApiGateway.Collect;
using HttpApiGateway.Helper;
using HttpApiGateway.Interface;
using HttpApiGateway.Model;

using RpcHelper;

namespace HttpApiGateway.ActionFun
{
        internal class BasicFunc : IHttpApi
        {
                public string ApiName { get; }

                protected readonly MethodInfo _Method;
                public string ApiUri { get; }

                private readonly Type _SourceType = null;

                private readonly string _ApiShow = null;

                protected readonly FuncParam[] _Params;

                protected readonly int _CountIndex = -1;

                protected readonly bool _IsPagingFun = false;
                public BasicFunc(ApiModel param)
                {
                        this._SourceType = param.Type;
                        this.ApiUri = param.ApiUri;
                        this.ApiName = param.Method.Name;
                        this._Method = param.Method;
                        this._ApiShow = string.Format("Path:{0}\r\nName:{1}\r\nMethod:{2}\r\nSource:{3}", this.ApiUri, this.ApiName, this._Method.Name, param.Type.FullName);
                        this._Params = FuncHelper.InitMethod(this._Method);
                        this._CountIndex = this._Params.FindIndex(a => a.ParamType == FuncParamType.数据总数);
                        this._IsPagingFun = this._CountIndex != -1;
                }

                private void _InvokeMethod(IService service, object[] param)
                {
                        IApiGateway api = UnityCollect.GetGateway(this._SourceType);
                        try
                        {
                                this.Invoke(service, api, param);
                        }
                        catch (Exception e)
                        {
                                ErrorException error = ErrorException.FormatError(e);
                                service.ReplyError(error.ToString());
                        }
                }
                protected virtual void Invoke(IService service, IApiGateway api, object[] param)
                {

                }
                public void ExecApi(IService service)
                {
                        if (!ApiHelper.InitMethodParam(this._Params, this._Method, service, out object[] param, out string error))
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
                                        ReceiveMethod = a.ReceiveMethod,
                                        ParamIndex = i
                                }),
                                ApiType = route.ApiType,
                                UpConfig = route.UpCheck.ToUpSet(),
                                IsPaging = _IsPagingFun,
                                Source = _SourceType,
                                IsAccredit = route.IsAccredit,
                                Method = _Method,
                                Prower = route.Prower,
                                ApiUri = route.ApiUri
                        };
                        this.InitApiBody(route, param);
                        ApiGatewayService.RegApi(param);
                }
                protected virtual void InitApiBody(IApiRoute route, ApiFuncBody body)
                {

                }
                public virtual bool VerificationApi()
                {
                        if (this._IsPagingFun && (!this._Method.ReturnType.IsArray && !this._Method.ReturnType.IsGenericType))
                        {
                                return false;
                        }
                        return true;
                }
        }
}
