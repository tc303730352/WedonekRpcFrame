using WeDonekRpc.Model.Model;

namespace WeDonekRpc.Model.Server
{
    public class ServerConfigInfo : RpcServerConfig
    {
        /// <summary>
        /// 集群ID
        /// </summary>
        public long RpcMerId { get; set; }
        /// <summary>
        /// 降级配置
        /// </summary>
        public ReduceInRank Reduce
        {
            get;
            set;
        }
        /// <summary>
        /// 限流配置
        /// </summary>
        public ServerClientLimit ClientLimit
        {
            get;
            set;
        }
    }
}
