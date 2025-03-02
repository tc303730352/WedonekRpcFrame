using WeDonekRpc.Helper.Validate;

namespace Store.Gatewary.Modular.Model
{
    internal class SetIsEnable<IdentityId> where IdentityId : struct
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        [NumValidate("public.param.id.error", 1)]
        public IdentityId Id { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        public bool IsEnable { get; set; }
    }
}
