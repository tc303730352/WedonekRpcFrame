using WeDonekRpc.Modular.Model;

namespace WeDonekRpc.ApiGateway.Model
{
    /// <summary>
    /// 客户端标识
    /// </summary>
    internal class ClientIdentity : Interface.IClientIdentity
    {
        public ClientIdentity ()
        {
            string identity = GatewayServer.UserIdentity.IdentityId;
            if (identity != null)
            {
                this.IdentityId = identity;
            }
        }
        /// <summary>
        /// 身份标识
        /// </summary>
        public string IdentityId { get; }
        /// <summary>
        /// 应用名称
        /// </summary>
        public UserIdentity Identity => GatewayServer.UserIdentity.GetIdentity();
    }
}
