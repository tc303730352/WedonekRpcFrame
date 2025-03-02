namespace RpcSync.Model
{
    public class RemoteServer
    {
        public long Id { get; set; }

        /// <summary>
        /// 服务类别Id
        /// </summary>
        public long SystemType
        {
            get;
            set;
        }
        /// <summary>
        /// 所属容器Id
        /// </summary>
        public long? ContainerId
        {
            get;
            set;
        }
        /// <summary>
        /// 是否为容器
        /// </summary>
        public bool IsContainer
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
        /// 节点版本号
        /// </summary>
        public int VerNum
        {
            get;
            set;
        }
    }
}
