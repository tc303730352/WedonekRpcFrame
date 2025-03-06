using System;
using System.Threading;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Client.Track.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpApiGateway.Collect;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpService.FileUp;
using WeDonekRpc.HttpService.Handler;
namespace WeDonekRpc.HttpApiGateway.Handler
{

    internal class HttpApiHandler : BasicHandler, IApiHandler
    {
        private readonly IApiRoute _Route = null;
        private static readonly IIocService _Ioc = RpcClient.Ioc;
        private IUpFileConfig _UpConfig;
        public static AsyncLocal<IService> ApiService = new AsyncLocal<IService>(_ApiServiceChange);
        private IService _ApiService = null;
        private IocScope _Scope;
        public HttpApiHandler ( IApiRoute route ) : base(route.ApiUri, 99, route.IsRegex)
        {
            this._Route = route;
        }
        private static void _ApiServiceChange ( AsyncLocalValueChangedArgs<IService> e )
        {
            if ( e.CurrentValue != null && e.CurrentValue.IsEnd )
            {
                ApiService.Value = null;
            }
        }
        public override void ExecError ( Exception error )
        {
            ErrorException ex = ErrorException.FormatError(error);
            this._ApiService.ReplyError(ex.ToString());
            if ( ex.IsSystemError )
            {
                ex.Save("HttpGateway");
            }
        }
        protected sealed override bool InitHandler ()
        {
            GatewayApiService.Init(this, this._Route);
            if ( ApiPlugInService.Request_Init(this._Route, this) )
            {
                this._Scope = _Ioc.CreateScore();
                this._ApiService = new ApiService(this, this._Route, this._Scope);
                HttpApiHandler.ApiService.Value = this._ApiService;
                this._Init();
                return !this._ApiService.IsError;
            }
            return false;
        }
        private void _Init ()
        {
            GatewayApiService.BeginInit(this._ApiService, this._Route);
            this._UpConfig = this._Route.CreateUpFileConfig(this._Scope);
            base.IsGenerateMd5 = this._UpConfig.UpSet.IsGenerateMd5;
            this._Route.ReceiveRequest(this._ApiService);
            GatewayApiService.EndInit(this._ApiService, this._Route);
        }
        protected override bool CheckCache ( string etag, string toUpdateTime )
        {
            return this._ApiService.CheckCache(etag, toUpdateTime);
        }
        public override void Execute ()
        {
            GatewayApiService.RouteBegin(this._ApiService, this._Route);
            this._Execute();
            GatewayApiService.RouteEnd(this._ApiService, this._Route);
        }
        private void _Execute ()
        {
            if ( !GatwewayTrackCollect.CheckIsTrace(this._ApiService, out long spanId) )
            {
                this._Route.ExecApi(this._ApiService);
            }
            else
            {
                using ( TrackBody track = GatwewayTrackCollect.CreateTrack(this._ApiService, spanId) )
                {
                    this._Route.ExecApi(this._ApiService);
                    GatwewayTrackCollect.EndTrack(track, this._ApiService);
                }
            }
        }
        public override void VerificationFile ( UpFileParam param, long fileSize )
        {
            this._UpConfig.CheckFileSize(this._ApiService, fileSize);
        }
        public override void InitFileUp ()
        {
            this._UpConfig.InitFileUp(this._ApiService);
        }
        public override void CheckUpFile ( UpFileParam param )
        {
            this._UpConfig.CheckUpFormat(this._ApiService, param.FileName, this.Request.Files.Count + 1);
        }
        public override void End ()
        {
            this._ApiService?.End();
            base.End();
        }
        public override void Dispose ()
        {
            GatewayApiService.EndRequest(this, this._Route, this._ApiService);
            if ( this._ApiService != null )
            {
                this._ApiService.Dispose();
                this._ApiService = null;
                ApiService.Value = null;
            }
            this._Scope?.Dispose();
            base.Dispose();
        }
    }
}
