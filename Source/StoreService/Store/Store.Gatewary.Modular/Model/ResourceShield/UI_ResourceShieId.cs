using WeDonekRpc.Helper.Validate;
using System;
namespace Store.Gatewary.Modular.Model.ResourceShield
{
    /// <summary>
    /// 屏蔽已有资源 UI参数实体
    /// </summary>
    internal class UI_ResourceShieId
    {
        /// <summary>
        /// 资源ID
        /// </summary>
        [NumValidate("rpc.store.resourceshield.resourceId.error", 1)]
        public long ResourceId { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime? BeOverTime { get; set; }

        /// <summary>
        /// 服务节点
        /// </summary>
        public long[] ServerId
        {
            get;
            set;
        }

    }
}
