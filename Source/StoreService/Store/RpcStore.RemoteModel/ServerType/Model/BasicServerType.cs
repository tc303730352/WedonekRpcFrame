using WeDonekRpc.Model;

namespace RpcStore.RemoteModel.ServerType.Model
{
    public class BasicServerType
    {
        /// <summary>
        /// 类型ID
        /// </summary>
        public long Id
        {
            get;
            set;
        }

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
        /// 服务类型
        /// </summary>
        public RpcServerType ServerType
        {
            get;
            set;
        }
    }
}
