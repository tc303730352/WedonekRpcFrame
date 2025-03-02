using WeDonekRpc.Model;

namespace RpcCentral.Model
{
    public class ServerTransmitScheme
    {
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 方案名称
        /// </summary>
        public string Scheme
        {
            get;
            set;
        }

        /// <summary>
        /// 负载均衡类型
        /// </summary>
        public TransmitType TransmitType
        {
            get;
            set;
        }
        /// <summary>
        /// 版本号
        /// </summary>
        public int VerNum
        {
            get;
            set;
        }

        /// <summary>
        /// 负载配置
        /// </summary>
        public ServerTransmitConfig[] Transmit
        {
            get;
            set;
        }

    }
}
