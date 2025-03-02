using WeDonekRpc.Helper.Validate;

namespace RpcStore.RemoteModel.SysConfig.Model
{
    public class BasicGetParam
    {
        /// <summary>
        /// 集群ID
        /// </summary>
        [NumValidate("rpc.store.config.id.error", 1)]
        public long RpcMerId { get; set; }
        /// <summary>
        /// 配置项
        /// </summary>
        [NullValidate("rpc.store.config.name.null")]
        [LenValidate("rpc.store.config.name.len", 2, 50)]
        public string Name { get; set; }
    }
}
