using System;

using ApiGateway;
using ApiGateway.Attr;
using ApiGateway.Interface;

using HttpApiGateway.Handler;
using HttpApiGateway.Interface;
using HttpApiGateway.Model;

using RpcHelper;

namespace HttpApiGateway
{
        internal class ApiRoute : IApiRoute
        {
                private readonly IHttpApi _Api = null;

                private readonly IApiModular _Modular = null;

                private readonly IUpCheck _UpCheck = null;

                private readonly CheckCache _CacheVer = null;

                private readonly CheckFile _CheckFile = null;

                private readonly CheckFileSize _CheckSize = null;

                private readonly bool _IsAccredit = false;
                private readonly Action<IApiService> _AccreditVer = null;

                private readonly string _Prower = null;
                public ApiType ApiType { get; }


                public ApiRoute(IHttpApi api, ApiModel model, RouteConfig config, IApiModular modular)
                {
                        this._Api = api;
                        this._CacheVer = config.Cache;
                        this._AccreditVer = config.AccreditVer;
                        this._CheckFile = config.CheckFile;
                        this._CheckSize = config.CheckSize;
                        this._Modular = modular;
                        this.ApiType = model.ApiType;
                        this._UpCheck = model.UpCheck;
                        this._Prower = model.Prower;
                        this._IsAccredit = model.IsAccredit;
                        this.ApiUri = api.ApiUri;
                }
                public string ApiUri { get; }

                public string ServiceName => this._Modular.ServiceName;
                public bool IsAccredit => this._IsAccredit;

                public string Prower => this._Prower;

                public IUpCheck UpCheck => this._UpCheck;

                public void ExecApi(IService service)
                {
                        this._Api.ExecApi(service);
                }

                private void _CheckAccredit(IService service)
                {
                        if (this._AccreditVer != null)
                        {
                                this._AccreditVer(service);
                                if (service.UserState != null && !service.UserState.CheckPrower(this._Prower))
                                {
                                        throw new ErrorException("accredit.no.prower");
                                }
                        }
                        else if (service.UserState == null && this._IsAccredit)
                        {
                                throw new ErrorException("accredit.unauthorized");
                        }
                        else if (service.UserState != null)
                        {
                                if (!service.UserState.CheckPrower(this._Prower))
                                {
                                        throw new ErrorException("accredit.no.prower");
                                }
                        }

                }
                public bool ReceiveRequest(IService service)
                {
                        if (!ApiGatewayService.ReceiveRequest(service))
                        {
                                return false;
                        }
                        try
                        {
                                service.InitService(this._Modular);
                                this._CheckAccredit(service);
                                return true;
                        }
                        catch (Exception e)
                        {
                                ErrorException error = ErrorException.FormatError(e);
                                service.ReplyError(error.ToString());
                                return false;
                        }
                }

                public void CheckUpFormat(IApiService service, string fileName, int num)
                {
                        if (this._CheckFile != null)
                        {
                                this._CheckFile(service, fileName, num);
                        }
                        else
                        {
                                this._UpCheck.CheckUpFile(fileName, num);
                        }
                }
                public void InitRoute()
                {
                        HttpService.HttpService.AddRoute(new HttpApiHandler(this));
                        this._Api.RegApi(this);
                }
                public void CheckFileSize(IApiService service, long fileSize)
                {
                        if (this._CheckSize != null)
                        {
                                this._CheckSize(service, fileSize);
                        }
                        else
                        {
                                this._UpCheck.CheckFileSize(fileSize);
                        }
                }

                public bool CheckCache(IApiService service, string etag, string toUpdateTime)
                {
                        if (this._CacheVer != null)
                        {
                                return this._CacheVer(service, etag, toUpdateTime);
                        }
                        else if (toUpdateTime != null)
                        {
                                return DateTime.Parse(toUpdateTime) >= DateTime.Now;
                        }
                        return false;
                }
        }
}
