namespace RpcStore.RemoteModel.ClientLimit.Model
{
    public class ClientLimitModel
    {
        /// <summary>
        /// 集群ID
        /// </summary>
        public long RpcMerId { get; set; }

        /// <summary>
        /// 集群名
        /// </summary>
        public string RpcMerName { get; set; }

        /// <summary>
        /// 客户端限制
        /// </summary>
        public ClientLimitData Limit { get; set; }
    }
}
