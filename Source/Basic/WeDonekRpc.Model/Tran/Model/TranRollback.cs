namespace WeDonekRpc.Model.Tran.Model
{
    public class TranRollback
    {
        /// <summary>
        /// 事务名
        /// </summary>
        public string TranName
        {
            get;
            set;
        }
        /// <summary>
        /// 超时时间(时间戳)
        /// </summary>
        public long OverTime { get; set; }
        /// <summary>
        /// 提交的数据
        /// </summary>
        public string SubmitJson
        {
            get;
            set;
        }
        /// <summary>
        /// 扩展字段
        /// </summary>
        public string Extend { get; set; }
        /// <summary>
        /// 事务Id
        /// </summary>
        public long TranId { get; set; }
    }
}
