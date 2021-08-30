namespace RpcModel
{
        /// <summary>
        /// 服务节点配置
        /// </summary>
        public class ServerConfig
        {
                /// <summary>
                /// 节点Id
                /// </summary>
                public long ServerId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 是否启用负载均衡
                /// </summary>
                public bool IsTransmit
                {
                        get;
                        set;
                }
                /// <summary>
                /// 负载均衡配置
                /// </summary>
                public ServerTransmit[] Transmit
                {
                        get;
                        set;
                }
                /// <summary>
                /// 权重
                /// </summary>
                public int Weight
                {
                        get;
                        set;
                }

        }
}
