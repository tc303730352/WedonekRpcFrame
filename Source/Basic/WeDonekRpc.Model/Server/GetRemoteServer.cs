namespace WeDonekRpc.Model.Server
{
    public class LocalEnvironment
    {
        /// <summary>
        /// 当前用户身份标识
        /// </summary>
        public string RunUserIdentity { get; set; }
        /// <summary>
        /// 是否是管理员身份运行
        /// </summary>
        public bool RunIsAdmin { get; set; }

        public string Mac { get; set; }
    }
    /// <summary>
    /// 获取远程服务器
    /// </summary>
    public class GetRemoteServer
    {
        public long ServerId
        {
            get;
            set;
        }
        public long CurServerId
        {
            get;
            set;
        }
    }
}
