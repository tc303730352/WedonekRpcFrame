using WeDonekRpc.Client;
using WeDonekRpc.HttpWebSocket.Model;

using WeDonekRpc.Modular;

using WeDonekRpc.WebSocketGateway.Interface;

namespace WeDonekRpc.WebSocketGateway.Model
{
    internal class CurrentService : ICurrentService
    {
        private ICurrentModular _Modular = null;
        public CurrentService ()
        {
            IApiSocketService service = ApiHandler.ApiService.Value;
            if ( service != null )
            {
                this.AccreditId = service.AccreditId;
                this.Head = service.Head;
                this.Identity = service.Identity;
                this.IdentityId = service.IdentityId;
                this.ServiceName = service.ServiceName;
                this.Session = service.Session;
                this.UserState = service.UserState;
                this.IsHasValue = true;
                this.IsEnd = service.IsEnd;
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

        public IUserIdentity Identity { get; }

        public string IdentityId { get; }

        public string ServiceName { get; }

        public ISession Session { get; }

        public IUserState UserState { get; }
        public ICurrentModular Modular
        {
            get
            {
                this._Modular ??= RpcClient.Ioc.Resolve<ICurrentModular>(this.ServiceName);
                return this._Modular;
            }
        }

        public bool IsEnd { get; }
    }
}
