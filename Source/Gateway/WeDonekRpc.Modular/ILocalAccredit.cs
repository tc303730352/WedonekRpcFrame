namespace WeDonekRpc.Modular
{
    /// <summary>
    /// 本地授权码
    /// </summary>
    public interface ILocalAccredit
    {
        /// <summary>
        /// 当前授权码
        /// </summary>
        string AccreditId { get; }
        /// <summary>
        /// 取消授权
        /// </summary>
        void CancelAccredit();
        /// <summary>
        /// 检查是否授权
        /// </summary>
        void CheckAccredit();

        bool CheckIsAccredit();
        /// <summary>
        /// 获取用户状态
        /// </summary>
        /// <returns></returns>
        IUserState GetUserState();
    }
}