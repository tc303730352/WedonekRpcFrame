﻿using System.Threading;
using WeDonekRpc.Client.Track.Model;
using WeDonekRpc.HttpWebSocket.Interface;
using WeDonekRpc.WebSocketGateway.Collect;
using WeDonekRpc.WebSocketGateway.Interface;
using WeDonekRpc.WebSocketGateway.Model;

namespace WeDonekRpc.WebSocketGateway
{
    public class ApiHandler : System.IDisposable
    {
        private readonly IApiModular _Modular = null;

        private volatile bool _isEnable = true;
        public static AsyncLocal<IApiSocketService> ApiService = new AsyncLocal<IApiSocketService>(_ApiServiceChange);

        private Interface.IWebSocketService _ApiService = null;

        public string LocalPath => this.Route.LocalPath;
        public IApiRoute Route { get; } = null;

        public bool IsEnable => this._isEnable;

        internal ApiHandler ( ISocketApi api, ApiModel model, IRouteConfig config, IApiModular modular )
        {
            this.Id = RouteCollect.CreateApiId();
            this.Route = new ApiRoute(api, model, config, modular);
            this._Modular = modular;
        }
        private static void _ApiServiceChange ( AsyncLocalValueChangedArgs<IApiSocketService> e )
        {
            if ( e.CurrentValue != null && e.CurrentValue.IsEnd )
            {
                ApiService.Value = null;
            }
        }
        /// <summary>
        /// 路由ID
        /// </summary>
        public string Id { get; }
        public void Execute ( IApiService service, IUserPage page, byte[] content )
        {
            this._ApiService = new ApiSocketService(page, content, this._Modular, service);
            ApiService.Value = this._ApiService;
            this._ExecuteRoute();
        }
        private void _ExecuteRoute ()
        {
            if ( !GatwewayTrackCollect.CheckIsTrace(out long spanId) )
            {
                this.Route.ExecApi(this._ApiService);
                return;
            }
            using ( TrackBody track = GatwewayTrackCollect.CreateTrack(this._ApiService, this.LocalPath, spanId) )
            {
                this.Route.ExecApi(this._ApiService);
                GatwewayTrackCollect.EndTrack(track, this._ApiService);
            }
        }

        internal void Init ()
        {
            this.Route.RegApi();
        }

        public void Dispose ()
        {
            this._ApiService.Dispose();
            ApiService.Value = null;
        }

        internal void Enable ()
        {
            if ( !this._isEnable )
            {
                this._isEnable = true;
            }
        }
        internal void Disable ()
        {
            if ( this._isEnable )
            {
                this._isEnable = false;
            }
        }
    }
}
