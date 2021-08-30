using System;

using HttpWebSocket.Interface;

using RpcClient.Track.Model;

using WebSocketGateway.Collect;
using WebSocketGateway.Interface;
using WebSocketGateway.Model;

namespace WebSocketGateway
{
        internal class ApiHandler
        {
                private readonly IApiRoute _Route = null;
                private readonly IApiModular _Modular = null;
                [ThreadStatic]
                public static IApiSocketService ApiService = null;

                private Interface.IWebSocketService _ApiService = null;

                public string LocalPath { get; }

                public IApiRoute Route => this._Route;

                public ApiHandler(ISocketApi api, ApiModel model, IRouteConfig config, IApiModular modular)
                {
                        this._Route = new ApiRoute(api, model, config, modular); ;
                        this._Modular = modular;
                        this.LocalPath = api.LocalPath;
                }
                public void Execute(IApiService service, IUserPage page, byte[] content)
                {
                        this._ApiService = new ApiSocketService(page, content, this._Modular, service);
                        ApiService = this._ApiService;
                        this._ExecuteRoute();
                }
                private void _ExecuteRoute()
                {
                        if (!GatwewayTrackCollect.CheckIsTrace(out long spanId))
                        {
                                this._Route.ExecApi(this._ApiService);
                                return;
                        }
                        using (TrackBody track = GatwewayTrackCollect.CreateTrack(this._ApiService, this.LocalPath, spanId))
                        {
                                this._Route.ExecApi(this._ApiService);
                                GatwewayTrackCollect.EndTrack(track, this._ApiService);
                        }
                }

                internal void RegApi()
                {
                        this._Route.RegApi();
                }
        }
}
