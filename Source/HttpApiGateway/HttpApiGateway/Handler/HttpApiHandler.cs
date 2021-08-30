using System;

using HttpApiGateway.Collect;
using HttpApiGateway.Interface;

using HttpService;
using HttpService.Handler;

using RpcClient.Track.Model;

using RpcHelper;
namespace HttpApiGateway.Handler
{

        internal class HttpApiHandler : BasicHandler, IApiHandler
        {
                private readonly IApiRoute _Route = null;
                [ThreadStatic]
                public static IService ApiService = null;
                private IService _ApiService = null;
                public HttpApiHandler(IApiRoute route) : base(route.ApiUri, 99, false)
                {
                        this._Route = route;
                }

                protected sealed override bool InitHandler()
                {
                        this._ApiService = new ApiService(this, this._Route.ServiceName);
                        HttpApiHandler.ApiService = this._ApiService;
                        return this._Route.ReceiveRequest(this._ApiService);
                }

                protected override bool CheckCache(string etag, string toUpdateTime)
                {
                        return this._Route.CheckCache(this._ApiService, etag, toUpdateTime);
                }
                public override void Execute()
                {
                        if (!GatwewayTrackCollect.CheckIsTrace(this._ApiService, out long spanId))
                        {
                                this._Route.ExecApi(this._ApiService);
                                return;
                        }
                        using (TrackBody track = GatwewayTrackCollect.CreateTrack(this._ApiService, spanId))
                        {
                                this._Route.ExecApi(this._ApiService);
                                GatwewayTrackCollect.EndTrack(track, this._ApiService);
                        }
                }
                public override bool VerificationFile(UpFileParam param, long fileSize)
                {
                        try
                        {
                                this._Route.CheckFileSize(this._ApiService, fileSize);
                                return true;
                        }
                        catch (Exception e)
                        {
                                ErrorException ex = ErrorException.FormatError(e);
                                this._ApiService.ReplyError(ex.ToString());
                                return false;
                        }
                }

                public override bool CheckUpFile(UpFileParam param, int num)
                {
                        try
                        {
                                this._Route.CheckUpFormat(this._ApiService, param.FileName, num);
                                return true;
                        }
                        catch (Exception e)
                        {
                                ErrorException ex = ErrorException.FormatError(e);
                                this._ApiService.ReplyError(ex.ToString());
                                return false;
                        }
                }
        }
}
