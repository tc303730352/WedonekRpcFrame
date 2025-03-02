using WeDonekRpc.Helper.Validate;
using RpcStore.RemoteModel.Identity.Model;
using System;
namespace Store.Gatewary.Modular.Model.Identity
{
    /// <summary>
    /// 修改身份标识 UI参数实体
    /// </summary>
    internal class UI_SetIdentity
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        [NumValidate("rpc.store.identity.id.null",1)]
        public long Id { get; set; }

        /// <summary>
        /// 身份标识资料
        /// </summary>
        [NullValidate("rpc.store.identity.datum.null")]
        public IdentityDatum Datum { get; set; }

    }
}
