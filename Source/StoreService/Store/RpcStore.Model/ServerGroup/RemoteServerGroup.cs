using WeDonekRpc.Model;

namespace RpcStore.Model.ServerGroup
{
    public class RemoteServerGroup
    {
        /// <summary>
        /// 服务Id
        /// </summary>
        public long ServerId
        {
            get;
            set;
        }
        public int RegionId
        {
            get;
            set;
        }

        /// <summary>
        /// 节点类型
        /// </summary>
        public long SystemType
        {
            get;
            set;
        }
        /// <summary>
        /// 类型值
        /// </summary>
        public string TypeVal
        {
            get;
            set;
        }
        /// <summary>
        /// 服务类型
        /// </summary>
        public RpcServerType ServiceType { get; set; }
    }
}
