namespace WeDonekRpc.Model.Server
{
    /// <summary>
    /// RPC服务器登陆
    /// </summary>
    public class RpcServerLogin
    {
        /// <summary>
        /// 获取TokenId
        /// </summary>
        public string AccessToken
        {
            get;
            set;
        }
        /// <summary>
        /// 远程服务登陆信息
        /// </summary>
        public RemoteServerLogin ServerLogin
        {
            get;
            set;
        }
        public string ApiVer { get; set; }
    }
}
