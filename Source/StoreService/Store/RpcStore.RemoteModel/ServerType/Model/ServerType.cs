using WeDonekRpc.Model;

namespace RpcStore.RemoteModel.ServerType.Model
{
    /// <summary>
    /// 服务类型
    /// </summary>
    public class ServerType
    {
        /// <summary>
        /// 服务类型ID
        /// </summary>
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 服务类别组ID
        /// </summary>
        public long GroupId
        {
            get;
            set;
        }
        /// <summary>
        /// 类型值
        /// </summary>
        public string TypeVal
        {
            get;
            set;
        }
        /// <summary>
        /// 服务名
        /// </summary>
        public string SystemName
        {
            get;
            set;
        }

        /// <summary>
        /// 默认端口
        /// </summary>
        public int DefPort
        {
            get;
            set;
        }
        /// <summary>
        /// 服务类型
        /// </summary>
        public RpcServerType ServiceType
        {
            get;
            set;
        }
    }
}
