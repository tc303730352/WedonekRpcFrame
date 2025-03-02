using System.Collections.Generic;
using WeDonekRpc.Helper.Validate;
namespace Store.Gatewary.Modular.Model.ServerBind
{
    /// <summary>
    /// 设置绑定的服务节点负载均衡时的权重 UI参数实体
    /// </summary>
    internal class UI_SetBindServerWeight
    {
        /// <summary>
        /// 负载均衡权重
        /// </summary>
        [NullValidate("rpc.store.bind.weight.null")]
        public Dictionary<long, int> Weight { get; set; }

    }
}
