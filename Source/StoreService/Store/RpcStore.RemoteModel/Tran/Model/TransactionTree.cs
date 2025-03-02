namespace RpcStore.RemoteModel.Tran.Model
{
    public class TransactionTree : TransactionLog
    {
        /// <summary>
        /// 集群Id
        /// </summary>
        public long RpcMerId
        {
            get;
            set;
        }
        /// <summary>
        /// 集群Id
        /// </summary>
        public string RpcMerName
        {
            get;
            set;
        }
        /// <summary>
        /// 下级事务
        /// </summary>
        public TransactionTree[] Children
        {
            get;
            set;
        }
    }
}
