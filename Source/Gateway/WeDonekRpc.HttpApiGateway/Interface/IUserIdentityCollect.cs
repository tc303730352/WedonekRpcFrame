using WeDonekRpc.Client.Attr;

namespace WeDonekRpc.HttpApiGateway.Interface
{
    /// <summary>
    /// 用户身份标识
    /// </summary>
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    public interface IUserIdentityCollect
    {
        /// <summary>
        /// 是否启用身份标识
        /// </summary>
        bool IsEnableIdentity { get; }
        /// <summary>
        /// 初始化身份标识并检查权限
        /// </summary>
        /// <param name="service">身份ID</param>
        void InitIdentity (IApiService service);
        /// <summary>
        /// 检查身份标识
        /// </summary>
        /// <param name="identityId"></param>
        void CheckIdentity (string identityId);

        /// <summary>
        /// 清除标识
        /// </summary>
        void ClearIdentity ();
    }
}