namespace RpcCentral.Model
{
    public class ServerTransmitConfig
    {
        /// <summary>
        /// 服务节点Id
        /// </summary>
        public string ServerCode
        {
            get;
            set;
        }

        /// <summary>
        /// 负载配置
        /// </summary>
        public TransmitConfig[] TransmitConfig
        {
            get;
            set;
        }
    }
}
