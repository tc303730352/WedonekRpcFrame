using WeDonekRpc.Modular.Model;

namespace WeDonekRpc.Modular
{
    /// <summary>
    /// 授权服务模块
    /// </summary>
    public interface IAccreditService
    {
        /// <summary>
        /// 当前登陆的授权码
        /// </summary>
        string AccreditId
        {
            get;
        }
        /// <summary>
        /// 当前登陆用户
        /// </summary>
        IUserState CurrentUser { get; }
        /// <summary>
        /// 添加授权
        /// </summary>
        /// <param name="applyId">用户唯一标识码</param>
        /// <param name="state">注册的状态数据</param>
        /// <param name="expire">授权有效期(秒)</param>
        /// <returns>授权码</returns>
        string AddAccredit (string applyId, UserState state, int? expire = null);
        /// <summary>
        /// 申请临时授权码
        /// </summary>
        /// <param name="isInherit">是否继承上级授权码的状态和权限</param>
        /// <param name="expire">授权有效期(秒)</param>
        /// <returns>临时授权码</returns>
        string ApplyTempAccredit (bool isInherit, int? expire = null);

        string ApplyTempAccredit (string applyId, UserState state, bool isInherit, int? expire = null);
        /// <summary>
        /// 取消授权
        /// </summary>
        /// <param name="accreditId">授权码</param>
        void CancelAccredit (string accreditId);
        /// <summary>
        /// 踢出授权用户
        /// </summary>
        /// <param name="applyId">用户唯一标识码</param>
        void KickOutAccredit (string applyId);

        /// <summary>
        /// 更新授权状态
        /// </summary>
        /// <param name="accreditId">授权码</param>
        /// <param name="state">授权状态</param>
        /// <param name="expire">授权过期时间(秒)</param>
        void ToUpdate (string accreditId, UserState state, int? expire = null);

        /// <summary>
        /// 检查是否授权
        /// </summary>
        /// <param name="accreditId">授权码</param>
        /// <returns>是否授权</returns>
        bool CheckIsAccredit (string accreditId);
        /// <summary>
        /// 检查授权状态
        /// </summary>
        /// <param name="accreditId">授权码</param>
        void CheckAccredit (string accreditId);
        /// <summary>
        /// 获取授权的状态数据
        /// </summary>
        /// <param name="accreditId">授权码</param>
        /// <returns>注册的授权状态信息</returns>
        IUserState GetAccredit (string accreditId);
    }
}