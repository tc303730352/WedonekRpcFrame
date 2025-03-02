namespace RpcStore.RemoteModel.Tran.Model
{
    public class TransactionData : TransactionLog
    {
        /// <summary>
        /// 父级事务Id
        /// </summary>
        public long ParentId
        {
            get;
            set;
        }
        /// <summary>
        /// 集群Id
        /// </summary>
        public long RpcMerId
        {
            get;
            set;
        }
        /// <summary>
        /// 节点类别
        /// </summary>
        public string SystemType
        {
            get;
            set;
        }

        /// <summary>
        /// 区域Id
        /// </summary>
        public int RegionId
        {
            get;
            set;
        }
        /// <summary>
        /// 提交的数据
        /// </summary>
        public string SubmitJson
        {
            get;
            set;
        }
        /// <summary>
        /// 扩展参数
        /// </summary>
        public string Extend
        {
            get;
            set;
        }
        /// <summary>
        /// 是否是根事务
        /// </summary>
        public bool IsRoot { get; set; }
    }
}
