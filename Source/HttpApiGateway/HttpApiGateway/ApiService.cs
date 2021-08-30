using System;

using ApiGateway;
using ApiGateway.Interface;

using HttpApiGateway.Helper;
using HttpApiGateway.Interface;

using HttpService.Interface;

using RpcModular;

namespace HttpApiGateway
{
        internal class ApiService : IService
        {
                private readonly IApiHandler _Handler = null;
                private readonly IApiResponseTemplate _Template = null;
                public ApiService(IApiHandler handler, string serviceName)
                {
                        this._Handler = handler;
                        this._Template = ApiGatewayService.Config.ApiTemplate;
                        this.ServiceName = serviceName;
                }

                private string _AccreditId = null;
                public string AccreditId
                {
                        get
                        {
                                if (this._AccreditId == null)
                                {
                                        this._AccreditId = ApiHelper.GetAccreditId(this._Handler.Request);
                                }
                                return this._AccreditId;
                        }
                }
                private IUserState _UserState = null;
                public IUserState UserState => this._UserState;

                public Uri UrlReferrer => this._Handler.Request.UrlReferrer;

                public IHttpRequest Request => this._Handler.Request;

                public IHttpResponse Response => this._Handler.Response;

                public string ServiceName { get; private set; }

                public bool IsEnd => this.Response.IsEnd;

                public bool IsError { get; private set; }

                public string LastError { get; private set; }

                public IClientIdentity Identity => RpcClient.RpcClient.Unity.Resolve<IClientIdentity>();



                public void ReplyError(string error)
                {
                        this.IsError = true;
                        this.LastError = error;
                        this.Reply(this._Template.GetErrorResponse(error));
                }
                public void Reply(IResponse response)
                {
                        if (response.Verification(this))
                        {
                                response.InitResponse(this);
                                response.WriteStream(this);
                        }
                }

                public void Reply(object result, object count)
                {
                        this.Reply(this._Template.GetResponse(result, count));
                }

                public void Reply()
                {
                        this.Reply(this._Template.GetResponse());
                }

                public void Reply(object result)
                {
                        if (result == null)
                        {
                                this.Reply();
                                return;
                        }
                        this.Reply(this._Template.GetResponse(result));
                }

                public void ReplyError(string show, Exception e)
                {
                        this.IsError = true;
                        this.LastError = "http.500";
                        this.Reply(this._Template.GetErrorResponse(this.LastError));
                }

                public void InitService(IApiModular modular)
                {
                        string identityId = this._Handler.Request.Headers["identityId"];
                        GatewayServer.UserIdentity.InitIdentity(identityId, this.Request.Url.LocalPath);
                        if (this.AccreditId != null)
                        {
                                this._UserState = modular.Config.GetAccredit(this.AccreditId);
                        }
                }
        }
}
