namespace RpcService.Model.DAL_Model
{
        /// <summary>
        /// 服务节点配置
        /// </summary>
        [System.Serializable]
        internal class BasicServer
        {
                /// <summary>
                /// 服务节点Id
                /// </summary>
                public long Id
                {
                        get;
                        set;
                }
                /// <summary>
                /// 节点的MAC
                /// </summary>
                public string ServerMac
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
                /// 负载均衡配置
                /// </summary>
                public TransmitConfig[] TransmitConfig
                {
                        get;
                        set;
                }
                /// <summary>
                /// 权重
                /// </summary>
                public int Weight { get; set; }
        }
}
