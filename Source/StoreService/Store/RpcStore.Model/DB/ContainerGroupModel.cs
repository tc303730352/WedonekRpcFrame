using SqlSugar;
using WeDonekRpc.Model;

namespace RpcStore.Model.DB
{
    [SugarTable("ContainerGroup")]
    public class ContainerGroupModel
    {
        /// <summary>
        /// ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
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
        /// 主机MAC
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

        /// <summary>
        /// 主机IP
        /// </summary>
        public string HostIp { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
