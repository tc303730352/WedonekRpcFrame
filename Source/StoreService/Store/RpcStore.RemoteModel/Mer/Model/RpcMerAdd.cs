using WeDonekRpc.Helper.Validate;
namespace RpcStore.RemoteModel.Mer.Model
{
    public class RpcMerAdd
    {
        /// <summary>
        /// 系统名
        /// </summary>
        [NullValidate("rpc.store.mer.name.null")]
        [LenValidate("rpc.store.mer.name.len", 2, 50)]
        public string SystemName
        {
            get;
            set;
        }
        /// <summary>
        /// 应用ID
        /// </summary>
        [NullValidate("rpc.store.mer.appId.null")]
        [LenValidate("rpc.store.mer.appId.len", 2, 50)]
        public string AppId
        {
            get;
            set;
        }
        /// <summary>
        /// 应用秘钥
        /// </summary>
        [NullValidate("rpc.store.mer.name.null")]
        [LenValidate("rpc.store.mer.name.len", 6, 100)]
        public string AppSecret
        {
            get;
            set;
        }
        /// <summary>
        /// 允许访问的服务器IP
        /// </summary>
        public string[] AllowServerIp
        {
            get;
            set;
        }
    }
}
