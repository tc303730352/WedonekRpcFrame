namespace RpcStore.RemoteModel.BroadcastErrorLog.Model
{
    public class MsgSourceDto
    {
        /// <summary>
        /// 服务节点类型
        /// </summary>
        public string SystemType { get; set; }
        /// <summary>
        /// 服务节点类型名
        /// </summary>
        public string SystemTypeName { get; set; }


        /// <summary>
        /// 来源服务名
        /// </summary>
        public string ServerName { get; set; }
        /// <summary>
        /// 所属集群
        /// </summary>
        public string RpcMer { get; set; }

        /// <summary>
        /// 所在区域
        /// </summary>
        public string Region { get; set; }
        /// <summary>
        /// 容器类型
        /// </summary>
        public string ContGroup { get; set; }
        /// <summary>
        /// 应用版本号
        /// </summary>
        public string VerNum { get; set; }
    }
}
