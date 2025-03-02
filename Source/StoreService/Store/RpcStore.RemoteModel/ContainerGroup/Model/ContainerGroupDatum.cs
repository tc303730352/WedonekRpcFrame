using WeDonekRpc.Model;

namespace RpcStore.RemoteModel.ContainerGroup.Model
{
    public class ContainerGroupDatum
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
        /// 容器编号
        /// </summary>
        public string Number
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
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
