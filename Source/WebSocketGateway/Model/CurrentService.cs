
using ApiGateway.Interface;

using HttpWebSocket.Model;

using RpcModular;

using WebSocketGateway.Interface;

namespace WebSocketGateway.Model
{
        internal class CurrentService : ICurrentService
        {
                private ICurrentModular _Modular = null;
                public CurrentService()
                {
                        IApiSocketService service = ApiHandler.ApiService;
                        if (service != null)
                        {
                                this.AccreditId = service.AccreditId;
                                this.Head = service.Head;
                                this.Identity = service.Identity;
                                this.IdentityId = service.IdentityId;
                                this.ServiceName = service.ServiceName;
                                this.Session = service.Session;
                                this.UserState = service.UserState;
                                this.IsHasValue = true;
                        }
                }
                /// <summary>
                /// 是否有值
                /// </summary>
                public bool IsHasValue
                {
                        get;
                }
                public string AccreditId { get; }

                public RequestBody Head { get; }

                public IClientIdentity Identity { get; }

                public string IdentityId { get; }

                public string ServiceName { get; }

                public ISession Session { get; }

                public IUserState UserState { get; }
                public ICurrentModular Modular
                {
                        get
                        {
                                if (this._Modular == null)
                                {
                                        this._Modular = RpcClient.RpcClient.Unity.Resolve<ICurrentModular>(this.ServiceName);
                                }
                                return this._Modular;
                        }
                }
        }
}
