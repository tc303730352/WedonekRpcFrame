namespace WeDonekRpc.Model.Tran.Model
{
    /// <summary>
    /// 事务日志
    /// </summary>
    public class TranLog
    {
        /// <summary>
        /// 事务名称
        /// </summary>
        public string TranName
        {
            get;
            set;
        }
        /// <summary>
        /// 所属集群ID
        /// </summary>
        public long RpcMerId
        {
            get;
            set;
        }
        /// <summary>
        /// 处理的节点ID
        /// </summary>
        public long ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 节点类型
        /// </summary>
        public string SystemType
        {
            get;
            set;
        }
    }
}
