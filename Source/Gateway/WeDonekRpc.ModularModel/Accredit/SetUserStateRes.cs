namespace WeDonekRpc.ModularModel.Accredit
{
    /// <summary>
    /// 修改授权状态结果
    /// </summary>
    public class SetUserStateRes
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess
        {
            get;
            set;
        }
        /// <summary>
        /// 当前版本号
        /// </summary>
        public int StateVer
        {
            get;
            set;
        }
        /// <summary>
        /// 用户状态值
        /// </summary>
        public string UserState
        {
            get;
            set;
        }
    }
}
