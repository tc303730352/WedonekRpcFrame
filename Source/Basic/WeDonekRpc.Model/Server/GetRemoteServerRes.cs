namespace WeDonekRpc.Model.Server
{
    /// <summary>
    /// 获取远程服务器配置
    /// </summary>
    public class GetRemoteServerRes
    {
        /// <summary>
        /// 
        /// </summary>
        public GetRemoteServerRes ()
        {

        }

        /// <summary>
        /// 服务器配置
        /// </summary>
        public RpcServerConfig ServerConfig
        {
            get;
            set;
        }
    }
}
