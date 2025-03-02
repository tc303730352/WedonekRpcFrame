using WeDonekRpc.Modular.Model;

namespace WeDonekRpc.ApiGateway.Interface
{
    /// <summary>
    /// 客户端标识
    /// </summary>
    public interface IClientIdentity
    {
        /// <summary>
        /// 客户端标识
        /// </summary>
        string IdentityId
        {
            get;
        }
        /// <summary>
        /// 标识信息
        /// </summary>
        UserIdentity Identity { get; }
    }
}
