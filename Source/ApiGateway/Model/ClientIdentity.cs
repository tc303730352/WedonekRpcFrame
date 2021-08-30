using ApiGateway.Collect;

using RpcModular.Domain;

namespace ApiGateway.Model
{
        /// <summary>
        /// 客户端标识
        /// </summary>
        internal class ClientIdentity : Interface.IClientIdentity
        {
                public ClientIdentity()
                {
                        IdentityDomain identity = UserIdentityCollect.Identity;
                        if (identity != null)
                        {
                                this.IdentityId = identity.IdentityId;
                                this.AppName = identity.AppName;
                        }
                }
                /// <summary>
                /// 身份标识
                /// </summary>
                public string IdentityId { get; }
                /// <summary>
                /// 应用名称
                /// </summary>
                public string AppName { get; }
        }
}
