using WeDonekRpc.Helper.Validate;
using System;
namespace Store.Gatewary.Modular.Model.VisitCensus
{
    /// <summary>
    /// 设置服务节点的访问统计备注信息 UI参数实体
    /// </summary>
    internal class UI_SetVisitCensusShow
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        [NumValidate("rpc.store.visitcensus.id.null",1)]
        public long Id { get; set; }

        /// <summary>
        /// 备注说明
        /// </summary>
        [NullValidate("rpc.store.visitcensus.show.null")]
        public string Show { get; set; }

    }
}
