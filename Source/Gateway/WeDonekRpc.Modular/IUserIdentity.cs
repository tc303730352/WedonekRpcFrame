using WeDonekRpc.Modular.Model;

namespace WeDonekRpc.Modular
{
    /// <summary>
    /// 身份标识
    /// </summary>
    public interface IUserIdentity
    {
        /// <summary>
        /// 应用Id
        /// </summary>
        string IdentityId { get; }
        /// <summary>
        /// 是否启用了身份标识
        /// </summary>
        bool IsEnableIdentity { get; }

        /// <summary>
        /// 获取租户信息
        /// </summary>
        /// <returns></returns>
        UserIdentity GetIdentity ();
        /// <summary>
        /// 设置身份标识ID
        /// </summary>
        /// <param name="identityId"></param>
        void SetIdentityId (string identityId);

    }
}
