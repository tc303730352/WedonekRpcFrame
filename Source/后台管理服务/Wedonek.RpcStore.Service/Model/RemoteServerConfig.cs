namespace Wedonek.RpcStore.Service.Model
{
        /// <summary>
        /// 服务结点
        /// </summary>
        public class RemoteServerConfig : ServerConfigDatum
        {

                /// <summary>
                /// 分发配置
                /// </summary>
                public TransmitConfig[] TransmitConfig
                {
                        get;
                        set;
                }

        }
}
