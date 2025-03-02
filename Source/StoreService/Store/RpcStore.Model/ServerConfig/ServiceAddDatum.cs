using WeDonekRpc.Model;
using RpcStore.RemoteModel.ServerConfig.Model;

namespace RpcStore.Model.ServerConfig
{
    public class ServiceAddDatum : ServerConfigAdd
    {
        /// <summary>
        /// 服务组别ID
        /// </summary>
        public long GroupId
        {
            get;
            set;
        }
        /// <summary>
        /// 服务类别
        /// </summary>
        public RpcServerType ServiceType { get; set; }
        /// <summary>
        /// 类别值
        /// </summary>
        public string SystemTypeVal { get; set; }
    }
}
