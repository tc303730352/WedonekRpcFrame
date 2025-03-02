using WeDonekRpc.Helper.Validate;
using WeDonekRpc.Model;
namespace RpcStore.RemoteModel.ServerBind.Model
{
    /// <summary>
    /// 服务绑定查询参数
    /// </summary>
    public class ServerBindQueryParam
    {
        /// <summary>
        /// 集群ID
        /// </summary>
        [NumValidate("rpc.store.mer.id.error", 1)]
        public long RpcMerId { get; set; }

        /// <summary>
        /// 所在机房ID
        /// </summary>
        public int? RegionId { get; set; }

        /// <summary>
        /// 容器组
        /// </summary>
        public long? ContainerGroup { get; set; }
        /// <summary>
        /// 服务类目
        /// </summary>
        public long? SystemTypeId { get; set; }

        /// <summary>
        /// 服务类型
        /// </summary>
        public RpcServerType? ServerType { get; set; }
        /// <summary>
        /// 服务状态
        /// </summary>
        public RpcServiceState[] ServiceState
        {
            get;
            set;
        }
        /// <summary>
        /// 是否是持有的节点
        /// </summary>
        public bool? IsHold { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public int? VerNum { get; set; }
    }
}
