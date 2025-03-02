using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpApiGateway.Model;

namespace WeDonekRpc.HttpApiGateway
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    public class ApiServiceEvent : IApiServiceEvent
    {
        private readonly IUserIdentityCollect _Identity;
        public ApiServiceEvent (IUserIdentityCollect identity)
        {
            this._Identity = identity;
        }
        internal ApiServiceEvent ( IocScope scope )
        {
            this._Identity = scope.Resolve<IUserIdentityCollect>();
        }
        public virtual bool CheckCache (IApiService service, string etag, string toUpdateTime)
        {
            return false;
        }

        public virtual void Dispose ()
        {
            this._Identity.ClearIdentity();
        }

        public virtual void EndRequest (IApiService service)
        {

        }

        public virtual void CheckAccredit (IApiService service, ApiAccreditSet set)
        {
            service.CheckAccredit(set);
        }
        public virtual void InitIdentity (IApiService service)
        {
            this._Identity.InitIdentity(service);
        }
        public virtual void InitRequest (IApiService service)
        {

        }

        public virtual void ReplyEvent (IApiService service, IResponse response)
        {
        }
    }
}
