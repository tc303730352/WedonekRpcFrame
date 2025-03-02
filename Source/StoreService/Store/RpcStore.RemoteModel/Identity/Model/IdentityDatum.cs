using WeDonekRpc.Helper.Validate;

namespace RpcStore.RemoteModel.Identity.Model
{
    public class IdentityDatum
    {
        /// <summary>
        /// 应用名
        /// </summary>
        [NullValidate("rpc.store.identity.name.null")]
        [LenValidate("rpc.store.identity.name.len", 2, 50)]
        public string AppName { get; set; }
        /// <summary>
        /// 应用说明
        /// </summary>
        [LenValidate("rpc.store.identity.show.len", 0, 255)]
        public string AppShow { get; set; }

        /// <summary>
        /// 应用扩展
        /// </summary>
        public Dictionary<string, string> AppExtend { get; set; }

        /// <summary>
        /// 应用有效期
        /// </summary>
        [TimeValidate("rpc.store.identity.effective.date.error", 1)]
        public DateTime? EffectiveDate { get; set; }

        /// <summary>
        /// 应用是否启用
        /// </summary>
        public bool IsEnable { get; set; }
    }
}
