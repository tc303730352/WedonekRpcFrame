using WeDonekRpc.Helper.Validate;

namespace RpcStore.RemoteModel.ServerPublic.Model
{
    public class SystemTypeVerScheme
    {
        /// <summary>
        /// 发布的版本号
        /// </summary>
        [NumValidate("rpc.store.scheme.ver.error", 0, int.MaxValue)]
        public int VerNum
        {
            get;
            set;
        }
        /// <summary>
        /// 发起节点类型ID
        /// </summary>
        [NumValidate("rpc.store.system.type.id.error", 1)]
        public long SystemTypeId
        {
            get;
            set;
        }
        /// <summary>
        /// 目标版本配置
        /// </summary>
        [NullValidate("rpc.store.scheme.to.ver.null")]
        public ToSystemTypeVer[] ToVer
        {
            get;
            set;
        }
    }
}
