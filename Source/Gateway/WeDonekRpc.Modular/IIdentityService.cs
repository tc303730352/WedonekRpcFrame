using WeDonekRpc.Modular.Model;

namespace WeDonekRpc.Modular
{
    /// <summary>
    /// 身份标识
    /// </summary>
    public interface IIdentityService
    {
        /// <summary>
        /// 身份标识ID
        /// </summary>
        string IdentityId { get; set; }
        /// <summary>
        /// 是否启用身份标识
        /// </summary>
        bool IsEnableIdentity
        {
            get;
        }
        /// <summary>
        /// 获取用户身份标识领域
        /// </summary>
        /// <returns></returns>
        UserIdentity GetIdentity ();

        /// <summary>
        /// 检查用户身份标识状态
        /// </summary>
        void CheckState ();
        void SetIdentityId (string identityId);
        void Clear ();
    }
}