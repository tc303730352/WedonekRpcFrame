using System;
using System.Reflection;
using WeDonekRpc.ApiGateway.Model;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpApiGateway.Collect;
using WeDonekRpc.HttpApiGateway.Helper;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpApiGateway.Model;

namespace WeDonekRpc.HttpApiGateway.ActionFun
{
    internal abstract class BasicFunc : IHttpApi
    {
        public string ApiName { get; }

        protected readonly MethodInfo _Method;
        public string ApiUri { get; }

        private readonly Type _SourceType = null;

        protected readonly FuncParam[] _Params;
        public MethodInfo Source => this._Method;
        public BasicFunc ( ApiModel param )
        {
            this._SourceType = param.Type;
            this.ApiUri = param.ApiUri;
            this.ApiName = param.Method.Name;
            this._Method = param.Method;
            this._Params = FuncHelper.InitMethod(this._Method);
        }
        private void _InvokeMethod ( IService service, object[] param )
        {
            IApiGateway api = UnityCollect.GetGateway(this._SourceType);
            try
            {
                this.Invoke(service, api, param);
            }
            catch ( Exception e )
            {
                ErrorException error = ErrorException.FormatError(e);
                error.SaveLog("HttpGateway");
                service.ReplyError(error.ToString());
            }
        }
        protected virtual void Invoke ( IService service, IApiGateway api, object[] param )
        {

        }
        public void ExecApi ( IService service )
        {
            if ( !ApiHelper.InitMethodParam(this._Params, this._Method, service, out object[] param, out string error) )
            {
                service.ReplyError(error);
            }
            else
            {
                this._InvokeMethod(service, param);
            }
        }

        public void RegApi ( IApiRoute route )
        {
            ApiFuncBody param = new ApiFuncBody
            {
                PostParam = this._Params.Convert(a => a.ParamType == FuncParamType.参数, ( a, i ) => new ApiPostParam
                {
                    Name = a.Name,
                    PostType = a.DataType,
                    ReceiveMethod = a.ReceiveMethod,
                    ParamIndex = i
                }),
                ApiType = route.ApiType,
                UpConfig = route.GetUpSet(),
                Source = this._SourceType,
                IsAccredit = route.IsAccredit,
                Method = this._Method,
                Prower = route.Prower,
                ApiUri = route.ApiUri
            };
            this.InitApiBody(route, param);
            ApiGatewayService.RegApi(param);
        }


        protected virtual void InitApiBody ( IApiRoute route, ApiFuncBody body )
        {

        }
    }
}
