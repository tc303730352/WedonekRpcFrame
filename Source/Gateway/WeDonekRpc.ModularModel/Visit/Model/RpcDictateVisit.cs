namespace WeDonekRpc.ModularModel.Visit.Model
{
    /// <summary>
    /// 节点API访问统计
    /// </summary>
    public class RpcDictateVisit
    {
        /// <summary>
        /// API名称
        /// </summary>
        public string Dictate { get; set; }
        /// <summary>
        /// 成功数
        /// </summary>
        public int Success { get; set; }
        /// <summary>
        /// 失败数
        /// </summary>
        public int Failure { get; set; }
        /// <summary>
        /// 平均请求量
        /// </summary>

        public int Concurrent { get; set; }
    }
}
