using System;
using System.Collections.Generic;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpApiGateway.Helper;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpApiGateway.Model;
using WeDonekRpc.HttpService.Interface;
using WeDonekRpc.Modular;

namespace WeDonekRpc.HttpApiGateway
{
    internal class ApiService : IService
    {
        private readonly IApiHandler _Handler = null;
        private readonly IApiServiceEvent _ApiEvent;
        private IAccredit _Accredit;
        private readonly IApiResponseTemplate _Template = null;
        public ApiService (IApiHandler handler, IApiRoute route, IocScope scope)
        {
            this.Scope = scope;
            this._Handler = handler;
            this._ApiEvent = scope.Resolve<IApiServiceEvent>(route.ApiEventType);
            this._Template = ApiGatewayService.Config.ApiTemplate;
            this.ServiceName = route.ServiceName;
        }
        public ApiService (IApiHandler handler, string name, IocScope scope )
        {
            this.Scope = scope;
            this._Handler = handler;
            this._Template = ApiGatewayService.Config.ApiTemplate;
            this._ApiEvent = new ApiServiceEvent(scope);
            this.ServiceName = name;
        }
        public IocScope Scope { get; private set; }

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
        private IState _State;
        public IUserState UserState
        {
            get
            {
                if (this._UserState == null && this.AccreditId.IsNotNull())
                {
                    this._UserState = this._Accredit.GetAccredit(this.AccreditId);
                }
                return this._UserState;
            }
        }

        public IState RequestState
        {
            get
            {
                if (this._State == null)
                {
                    this._State = new RequestState();
                }
                return this._State;
            }
        }
        public Uri UrlReferrer => this._Handler.Request.UrlReferrer;

        public IHttpRequest Request => this._Handler.Request;

        public IHttpResponse Response => this._Handler.Response;

        public string ServiceName { get; private set; }

        public bool IsEnd => this.Response.IsEnd;

        public bool IsError { get; private set; }

        public string LastError { get; private set; }

        public IUserIdentity Identity => this.Scope.Resolve<IUserIdentity>();

        public Dictionary<string, string> RouteArgs => this._Handler.RouteArgs;

        public Dictionary<string, string> PathArgs => this._Handler.PathArgs;

        public void ReplyError (string error)
        {
            this.IsError = true;
            this.LastError = error;
            this.Reply(this._Template.GetErrorResponse(error));
        }
        public void ReplyError (ErrorException error)
        {
            this.IsError = true;
            this.LastError = error.ErrorCode;
            this.Reply(this._Template.GetErrorResponse(this.LastError));
        }
        public void Reply (IResponse response)
        {
            if (!this.Response.IsEnd && response.Verification(this))
            {
                this._ApiEvent.ReplyEvent(this, response);
                response.InitResponse(this);
                response.WriteStream(this);
            }
        }
        public void Reply ()
        {
            this.Reply(this._Template.GetResponse());
        }

        public void Reply (object result)
        {
            if (result == null)
            {
                this.Reply();
                return;
            }
            this.Reply(this._Template.GetResponse(result));
        }

        public void ReplyError (string show, Exception e)
        {
            this.IsError = true;
            this.LastError = "http.500";
            this.Reply(this._Template.GetErrorResponse(this.LastError));
        }

        public void InitService (IApiModular modular)
        {
            this._Accredit = this.Scope.Resolve<IAccredit>();
            this._Accredit.SetAccreditId(this.AccreditId);
            this._ApiEvent.InitRequest(this);
        }
        public void InitIdentity ()
        {
            this._ApiEvent.InitIdentity(this);
        }

        public void CheckAccredit (ApiAccreditSet accreditSet)
        {
            this._ApiEvent.CheckAccredit(this, accreditSet);
        }

        public bool CheckCache (string etag, string toUpdateTime)
        {
            return this._ApiEvent.CheckCache(this, etag, toUpdateTime);
        }

        public void Dispose ()
        {
            this._Accredit.ClearAccredit();
            this._ApiEvent.Dispose();
        }

        public void End ()
        {
            if (!this._Handler.IsEnd)
            {
                this._ApiEvent.EndRequest(this);
            }
        }
    }
}
