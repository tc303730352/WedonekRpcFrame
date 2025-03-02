using System;
using System.Reflection;
using WeDonekRpc.ApiGateway;
using WeDonekRpc.ApiGateway.Model;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpApiGateway.Collect;
using WeDonekRpc.HttpApiGateway.FileUp;
using WeDonekRpc.HttpApiGateway.Handler;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpApiGateway.Model;

namespace WeDonekRpc.HttpApiGateway
{
    internal class ApiRoute : IApiRoute
    {
        private readonly IHttpApi _Api = null;
        private readonly IApiModular _Modular = null;

        private readonly IUpFileConfig _UpConfig;

        private readonly ApiAccreditSet _AccreditSet;
        private volatile bool _IsEnable = false;
        public ApiType ApiType { get; }

        public ApiRoute (IHttpApi api, ApiModel model, IApiModular modular)
        {
            this.Id = ApiRouteCollect.CreateApiId();
            this._Api = api;
            this.IsRegex = model.IsRegex;
            this._IsEnable = model.IsEnable;
            if (model.UpSet != null && model.UpConfig == null)
            {
                this._UpConfig = new UpFileConfig(model.UpSet);
            }
            else if (model.UpConfig == null)
            {
                this._UpConfig = DefUpFileConfig.CurConfig;
            }
            this._Modular = modular;
            this.ApiType = model.ApiType;
            this._AccreditSet = new ApiAccreditSet(model.IsAccredit, model.Prower);
            this.ApiEventType = model.ApiEventType == null ? modular.ServiceName : model.ApiEventType.FullName.GetMd5();
            this.ApiUri = api.ApiUri;
        }
        public string Id
        {
            get;
        }
        public string ApiUri { get; private set; }
        public MethodInfo Source => this._Api.Source;
        public string ServiceName => this._Modular.ServiceName;
        public bool IsAccredit => this._AccreditSet.IsAccredit;

        public string[] Prower => this._AccreditSet.Prower;

        public bool IsRegex { get; }

        public bool IsEnable => this._IsEnable;

        public string ApiEventType { get; }

        public void ExecApi (IService service)
        {
            this._Api.ExecApi(service);
        }
        public ApiUpSet GetUpSet ()
        {
            if (this._UpConfig != null)
            {
                return this._UpConfig.UpSet;
            }
            else
            {
                return null;
            }
        }
        public IUpFileConfig CreateUpFileConfig (IocScope scope)
        {
            if (this._UpConfig != null)
            {
                return this._UpConfig;
            }
            else
            {
                return scope.Resolve<IUpFileConfig>(this.Id);
            }
        }

        public void ReceiveRequest (IService service)
        {
            try
            {
                service.InitService(this._Modular);
                service.CheckAccredit(this._AccreditSet);
                service.InitIdentity();
            }
            catch (Exception e)
            {
                ErrorException error = ErrorException.FormatError(e);
                service.ReplyError(error.ToString());
                error.SaveLog("HttpGateway");
            }
        }
        public void InitRoute ()
        {
            this.ApiUri = ApiGateway.Helper.ApiHelper.FormatUri(this.ApiUri);
            if (this._IsEnable)
            {
                HttpService.HttpService.AddRoute(new HttpApiHandler(this));
            }
            this._Api.RegApi(this);
        }

        public void Dispose ()
        {
            HttpService.HttpService.RemoveRoute(this.ApiUri, this.IsRegex);
        }

        public void Enable ()
        {
            if (!this._IsEnable)
            {
                this._IsEnable = true;
                HttpService.HttpService.AddRoute(new HttpApiHandler(this));
            }
        }

        public void Disable ()
        {
            if (this._IsEnable)
            {
                this._IsEnable = false;
                HttpService.HttpService.RemoveRoute(this.ApiUri, this.IsRegex);
            }
        }
    }
}
