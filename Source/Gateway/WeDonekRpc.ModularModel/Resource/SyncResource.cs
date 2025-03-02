
using WeDonekRpc.Client;

using WeDonekRpc.Model;

using WeDonekRpc.ModularModel.Resource.Model;

namespace WeDonekRpc.ModularModel.Resource
{
    [IRemoteConfig("sys.sync")]
    public class SyncResource : RpcRemote
    {
        public string ModularName
        {
            get;
            set;
        }

        /// <summary>
        /// 资源类型
        /// </summary>
        public ResourceType ResourceType
        {
            get;
            set;
        }
        /// <summary>
        /// 资源列表
        /// </summary>
        public ResourceDatum[] Resources
        {
            get;
            set;
        }
    }
}
