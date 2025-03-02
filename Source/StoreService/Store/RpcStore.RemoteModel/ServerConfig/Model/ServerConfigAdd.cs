using WeDonekRpc.Helper.Validate;

namespace RpcStore.RemoteModel.ServerConfig.Model
{
    public class ServerConfigAdd : ServerConfigSet
    {
        /// <summary>
        /// 服务类别Id
        /// </summary>
        [NumValidate("rpc.store.server.type.error", 1)]
        public long SystemType
        {
            get;
            set;
        }
        /// <summary>
        /// 节点所在区域
        /// </summary>
        [NumValidate("rpc.store.region.id.error", 1)]
        public int RegionId
        {
            get;
            set;
        }

    }
}
