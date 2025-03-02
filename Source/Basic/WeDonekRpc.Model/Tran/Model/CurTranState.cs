namespace WeDonekRpc.Model.Tran.Model
{
    public class CurTranState
    {
        /// <summary>
        /// 事务Id
        /// </summary>
        public long TranId { get; set; }

        /// <summary>
        /// 事务协调服务所在区
        /// </summary>
        public int RegionId { get; set; }
        /// <summary>
        /// 事务协调服务所在集群
        /// </summary>
        public long RpcMerId { get; set; }
        /// <summary>
        /// 超时时间(时间戳)
        /// </summary>
        public long OverTime { get; set; }
    }
}
