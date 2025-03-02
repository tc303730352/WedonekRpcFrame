using WeDonekRpc.Model;

namespace RpcStore.RemoteModel.ServerBind.Model
{
    public class BindServerItem
    {
        /// <summary>
        /// 服务Id
        /// </summary>
        public long ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 服务名
        /// </summary>
        public string ServerName
        {
            get;
            set;
        }
        /// <summary>
        /// 服务编号
        /// </summary>
        public string ServerCode { get; set; }

        /// <summary>
        /// 服务状态
        /// </summary>
        public RpcServiceState ServiceState { get; set; }

        /// <summary>
        /// 是否为容器
        /// </summary>
        public bool IsContainer { get; set; }

        /// <summary>
        /// 服务Mac
        /// </summary>
        public string ServerMac
        {
            get;
            set;
        }
        /// <summary>
        /// 服务Ip
        /// </summary>
        public string ServerIp
        {
            get;
            set;
        }
        /// <summary>
        /// 服务端口
        /// </summary>
        public int ServerPort
        {
            get;
            set;
        }
    }
}
