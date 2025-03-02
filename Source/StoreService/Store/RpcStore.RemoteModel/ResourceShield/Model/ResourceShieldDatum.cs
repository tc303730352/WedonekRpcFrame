namespace RpcStore.RemoteModel.ResourceShield.Model
{
    public class ResourceShieldDatum
    {
        /// <summary>
        /// 数据Id
        /// </summary>
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 集群ID
        /// </summary>
        public long RpcMerId
        {
            get;
            set;
        }
        /// <summary>
        /// 系统类别
        /// </summary>
        public string SystemType
        {
            get;
            set;
        }
        /// <summary>
        /// 系统类别名
        /// </summary>
        public string SystemTypeName
        {
            get;
            set;
        }
        /// <summary>
        /// 服务节点ID
        /// </summary>
        public long ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 服务节点名
        /// </summary>
        public string ServerName
        {
            get;
            set;
        }
        /// <summary>
        /// 版本号
        /// </summary>
        public string ApiVer
        {
            get;
            set;
        }
        /// <summary>
        /// 优先级
        /// </summary>
        public short SortNum
        {
            get;
            set;
        }
        /// <summary>
        /// 资源路径
        /// </summary>
        public string ResourcePath
        {
            get;
            set;
        }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime? BeOverdueTime
        {
            get;
            set;
        }
        /// <summary>
        /// 屏蔽说明
        /// </summary>
        public string ShieIdShow { get; set; }
    }
}
