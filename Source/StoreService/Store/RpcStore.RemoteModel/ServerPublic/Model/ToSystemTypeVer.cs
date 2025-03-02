using WeDonekRpc.Helper.Validate;

namespace RpcStore.RemoteModel.ServerPublic.Model
{
    /// <summary>
    /// 目标服务节点版本
    /// </summary>
    public class ToSystemTypeVer
    {
        /// <summary>
        /// 服务节点类型
        /// </summary>
        [NumValidate("rpc.store.system.type.id.error", 1)]
        public long SystemTypeId
        {
            get;
            set;
        }
        /// <summary>
        /// 可用最小版本号
        /// </summary>
        [NumValidate("rpc.store.scheme.to.ver.error", 0, int.MaxValue)]
        public int ToVerId
        {
            get;
            set;
        }

    }
}
