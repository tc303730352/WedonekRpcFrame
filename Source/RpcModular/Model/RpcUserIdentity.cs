
using RpcClient.Attr;

using RpcModular;

namespace RpcModular.Model
{
        [ClassLifetimeAttr(ClassLifetimeType.单例)]
        internal class RpcUserIdentity : IUserIdentity
        {
                private readonly IIdentityService _Service = null;
                public RpcUserIdentity(IIdentityService service)
                {
                        this._Service = service;
                }
                public string AppId => this._Service.IdentityId;
        }
}
