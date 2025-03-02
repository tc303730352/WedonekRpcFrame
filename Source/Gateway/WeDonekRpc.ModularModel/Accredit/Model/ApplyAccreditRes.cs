namespace WeDonekRpc.ModularModel.Accredit.Model
{
    /// <summary>
    /// 申请授权结果
    /// </summary>
    public class ApplyAccreditRes
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public int StateVer
        {
            get;
            set;
        }
        /// <summary>
        /// 授权信息
        /// </summary>
        public AccreditRes Accredit
        {
            get;
            set;
        }
    }
}
