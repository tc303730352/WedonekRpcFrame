using WeDonekRpc.Model;

namespace RpcManageClient.Model
{
    public class ServiceDatum
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
        /// 节点类型
        /// </summary>
        public long SystemType
        {
            get;
            set;
        }
        /// <summary>
        /// 服务端口
        /// </summary>
        public int ServerPort
        {
            get;
            set;
        }
        /// <summary>
        /// 容器组ID
        /// </summary>
        public long? ContainerGroupId
        {
            get;
            set;
        }
        /// <summary>
        /// 节点MAC
        /// </summary>
        public string ServerMac
        {
            get;
            set;
        }
        /// <summary>
        /// 节点索引
        /// </summary>
        public int ServerIndex
        {
            get;
            set;
        }
        /// <summary>
        /// 服务状态
        /// </summary>
        public RpcServiceState ServiceState
        {
            get;
            set;
        }
        public long HoldRpcMerId
        {
            get;
            set;
        }
        public bool IsOnline
        {
            get;
            set;
        }
    }
}
