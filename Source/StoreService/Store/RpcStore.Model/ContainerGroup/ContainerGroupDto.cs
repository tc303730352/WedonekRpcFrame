using WeDonekRpc.Model;

namespace RpcStore.Model.ContainerGroup
{
    public class ContainerGroupDto
    {
        /// <summary>
        /// 容器组ID
        /// </summary>
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 容器宿主机IP
        /// </summary>
        public string HostIp
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
        /// 名字
        /// </summary>
        public string Name
        {
            get;
            set;
        }
    }
}
