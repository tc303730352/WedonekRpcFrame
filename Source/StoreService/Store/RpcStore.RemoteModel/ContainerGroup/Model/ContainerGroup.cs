using WeDonekRpc.Model;

namespace RpcStore.RemoteModel.ContainerGroup.Model
{
    public class ContainerGroup
    {
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 区域ID
        /// </summary>
        public int RegionId
        {
            get;
            set;
        }
        /// <summary>
        /// 区域名
        /// </summary>
        public string RegionName
        {
            get;
            set;
        }
        /// <summary>
        /// 宿主MAC
        /// </summary>
        public string HostMac
        {
            get;
            set;
        }
        /// <summary>
        /// 容器类型
        /// </summary>
        public ContainerType ContainerType
        {
            get;
            set;
        }
        /// <summary>
        /// 节点组名
        /// </summary>
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get;
            set;
        }
        public int ServerNum
        {
            get;
            set;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
