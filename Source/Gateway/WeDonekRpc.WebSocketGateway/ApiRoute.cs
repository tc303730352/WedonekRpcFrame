using System;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Helper;

using WeDonekRpc.WebSocketGateway.Interface;
using WeDonekRpc.WebSocketGateway.Model;

namespace WeDonekRpc.WebSocketGateway
{
    internal class ApiRoute : IApiRoute
    {
        private readonly ISocketApi _Api = null;

        private readonly IApiModular _Modular = null;

        private readonly bool _IsAccredit = false;

        private readonly Action<IApiSocketService> _AccreditVer = null;

        private readonly string[] _Prower = null;

        public ApiRoute (ISocketApi api, ApiModel model, IRouteConfig config, IApiModular modular)
        {
            this._Api = api;
            this._AccreditVer = config.AccreditVer;
            this._Modular = modular;
            this._Prower = model.Prower;
            this._IsAccredit = model.IsAccredit;
            this.LocalPath = api.LocalPath;
        }
        public string LocalPath { get; }

        public string ServiceName => this._Modular.ServiceName;
        public bool IsAccredit => this._IsAccredit;

        public string[] Prower => this._Prower;


        public void ExecApi (Interface.IWebSocketService service)
        {
            using (IocScope scope = RpcClient.Ioc.CreateScore())
            {
                service.InitService(this._Modular);
                this._CheckAccredit(service);
                this._Api.ExecApi(service);
            }
        }

        private void _CheckAccredit (Interface.IWebSocketService service)
        {
            if (this._AccreditVer != null)
            {
                this._AccreditVer(service);
            }
            if (!this._IsAccredit)
            {
                return;
            }
            else if (service.UserState == null)
            {
                throw new ErrorException("accredit.unauthorized");
            }
            else if (this._Prower != null && !service.UserState.CheckPrower(this._Prower))
            {
                throw new ErrorException("accredit.no.prower");
            }

        }


        public void RegApi ()
        {
            this._Api.RegApi(this);
        }
    }
}
