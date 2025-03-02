using WeDonekRpc.Model;

namespace RpcStore.RemoteModel.ServerConfig.Model
{
    public class ContainerShow
    {
        /// <summary>
        /// 所属容器组ID
        /// </summary>
        public long ContainerGroupId
        {
            get;
            set;
        }
        /// <summary>
        /// 容器宿主IP
        /// </summary>
        public string HostIp
        {
            get;
            set;
        }
        /// <summary>
        /// 简称
        /// </summary>
        public string Name
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
    }
}
