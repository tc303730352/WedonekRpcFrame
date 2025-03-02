using WeDonekRpc.Helper.Validate;
namespace RpcStore.RemoteModel.ServerPublic.Model
{
    public class PublicScheme
    {
        /// <summary>
        /// 方案名
        /// </summary>
        [NullValidate("rpc.store.scheme.name.null")]
        [LenValidate("rpc.store.scheme.name.len", 2, 50)]
        public string SchemeName { get; set; }


        /// <summary>
        /// 方案说明
        /// </summary>
        [LenValidate("rpc.store.scheme.show.len", 0, 100)]
        public string SchemeShow { get; set; }
        /// <summary>
        /// 发布节点版本配置
        /// </summary>
        public SystemTypeVerScheme[] Vers { get; set; }
    }
}
