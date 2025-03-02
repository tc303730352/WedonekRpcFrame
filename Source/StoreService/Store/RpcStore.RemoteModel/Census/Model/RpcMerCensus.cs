namespace RpcStore.RemoteModel.Census.Model
{
    internal class RpcMerCensus
    {
        /// <summary>
        /// 服务数
        /// </summary>
        public int ServerNum { get; set; }

        /// <summary>
        /// 在线服务
        /// </summary>
        public int Online { get; set; }
        /// <summary>
        /// 离线
        /// </summary>
        public int Offline { get; set; }
        /// <summary>
        /// 拥有的服务节点
        /// </summary>
        public int HoldNum { get; set; }
    }
}
