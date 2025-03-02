using System;
using System.Collections.Generic;
using System.Reflection;
using WeDonekRpc.ApiGateway.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.WebSocketGateway.Collect;
using WeDonekRpc.WebSocketGateway.Helper;
using WeDonekRpc.WebSocketGateway.Interface;
using WeDonekRpc.WebSocketGateway.Model;

namespace WeDonekRpc.WebSocketGateway.ActionFunc
{
    internal abstract class BasicFunc : ISocketApi
    {
        public string LocalPath { get; }
        private readonly bool _IsForm = false;

        protected readonly MethodInfo _Method;

        private readonly Type _SourceType = null;

        public string ApiName { get; }

        private readonly Dictionary<string, string> _ApiShow;

        protected readonly FuncParam[] _Params;
        public BasicFunc (ApiModel param)
        {
            this.LocalPath = param.LocalPath;
            this._SourceType = param.Type;
            this.ApiName = param.Method.Name;
            this._Method = param.Method;
            this._ApiShow = new Dictionary<string, string>
            {
                { "ApiName",this.ApiName },
                {"Method",this._Method.Name },
                { "Source",param.Type.FullName },
            };
            this._Params = FuncHelper.InitMethod(this._Method);
            this._IsForm = this._Params.Count(a => a.ParamType == FuncParamType.参数) > 1;
        }

        private void _InvokeMethod (IWebSocketService service, object[] param)
        {
            IApiGateway api = UnityCollect.GetGateway(this._SourceType);
            try
            {
                this.Invoke(service, api, param);
            }
            catch (Exception e)
            {
                ErrorException error = ErrorException.FormatError(e);
                error.AppendArg(this._ApiShow);
                GatewayLog.AddErrorLog(error);
                service.ReplyError(error.ToString());
            }
        }
        protected virtual void Invoke (IWebSocketService service, IApiGateway api, object[] param)
        {

        }
        public void ExecApi (IWebSocketService service)
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

        public void RegApi (IApiRoute route)
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
                Source = this._SourceType,
                IsAccredit = route.IsAccredit,
                Method = this._Method,
                Prower = route.Prower,
                ApiUri = route.LocalPath
            };
            this.InitApiBody(route, param);
            GatewayService.RegApi(param);
        }
        protected virtual void InitApiBody (IApiRoute route, ApiFuncBody body)
        {

        }

    }
}
