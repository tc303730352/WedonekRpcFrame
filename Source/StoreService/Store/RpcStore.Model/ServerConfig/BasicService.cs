using WeDonekRpc.Model;

namespace RpcStore.Model.ServerConfig
{
    public class BasicService
    {
        /// <summary>
        /// 服务Id
        /// </summary>
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 服务名
        /// </summary>
        public string ServerName
        {
            get;
            set;
        }
        /// <summary>
        /// 区域Id
        /// </summary>
        public int RegionId
        {
            get;
            set;
        }
        /// <summary>
        /// 服务类型
        /// </summary>
        public RpcServerType ServiceType { get; set; }

        public long SystemType { get; set; }

        public string ServerCode { get; set; }

        public int VerNum { get; set; }

        /// <summary>
        /// 拥有的集群ID(登陆)
        /// </summary>
        public long HoldRpcMerId
        {
            get;
            set;
        }
    }
}
