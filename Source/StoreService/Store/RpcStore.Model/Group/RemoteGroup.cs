namespace RpcStore.Model.Group
{
    /// <summary>
    /// 远程服务组别
    /// </summary>
    public class RemoteGroup
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
        /// 服务Id
        /// </summary>
        public long ServerId
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
        /// 类型值
        /// </summary>
        public string TypeVal
        {
            get;
            set;
        }
        /// <summary>
        /// 权重
        /// </summary>
        public int Weight
        {
            get;
            set;
        }
    }
}
