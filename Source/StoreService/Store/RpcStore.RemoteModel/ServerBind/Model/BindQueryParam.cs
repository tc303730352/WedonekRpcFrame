using WeDonekRpc.Model;

namespace RpcStore.RemoteModel.ServerBind.Model
{
    public class BindQueryParam
    {
        /// <summary>
        /// 查询关键字
        /// </summary>
        public string QueryKey
        {
            get;
            set;
        }
        /// <summary>
        /// 服务组Id
        /// </summary>
        public long? GroupId
        {
            get;
            set;
        }
        /// <summary>
        /// 服务类型Id
        /// </summary>
        public long? SystemTypeId
        {
            get;
            set;
        }
        /// <summary>
        /// 是否容器
        /// </summary>
        public bool? IsContainer
        {
            get;
            set;
        }
        /// <summary>
        /// 服务类型
        /// </summary>
        public RpcServerType? ServiceType
        {
            get;
            set;
        }
        /// <summary>
        /// 服务状态
        /// </summary>
        public RpcServiceState[] ServiceState
        {
            get;
            set;
        }
        /// <summary>
        /// 区域Id
        /// </summary>
        public int? RegionId
        {
            get;
            set;
        }
        /// <summary>
        /// 是否在线
        /// </summary>
        public bool? IsOnline
        {
            get;
            set;
        }

        public int? VerNum
        {
            get;
            set;
        }
        /// <summary>
        /// 是否属于
        /// </summary>
        public bool? IsHold
        {
            get;
            set;
        }
    }
}
