using System;

using RpcClient;

using RpcModel;

using RpcModularModel.Identity.Model;

namespace RpcModularModel.Identity
{
        [IRemoteConfig("sys.sync")]
        public class GetIdentity : RpcRemote<IdentityDatum>
        {
                public Guid IdentityId
                {
                        get;
                        set;
                }
        }
}
