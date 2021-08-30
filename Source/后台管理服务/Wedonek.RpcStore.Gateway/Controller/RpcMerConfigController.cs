using System;

using RpcHelper.Validate;

using Wedonek.RpcStore.Gateway.Interface;
using Wedonek.RpcStore.Gateway.Model;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Controller
{
        /// <summary>
        /// 集群配置
        /// </summary>
        internal class RpcMerConfigController : HttpApiGateway.ApiController
        {
                private readonly IRpcMerConfigService _Service = null;
                public RpcMerConfigController(IRpcMerConfigService service)
                {
                        this._Service = service;
                }
                /// <summary>
                /// 添加
                /// </summary>
                /// <param name="add"></param>
                /// <returns></returns>
                public Guid Add(AddMerConfig add)
                {
                        return this._Service.Add(add);
                }
                /// <summary>
                /// 删除
                /// </summary>
                /// <param name="id"></param>
                public void Drop([NullValidate("rpc.mer.config.id.null")] Guid id)
                {
                        this._Service.Drop(id);
                }
                /// <summary>
                /// 获取
                /// </summary>
                /// <param name="id"></param>
                /// <returns></returns>
                public RpcMerConfig Get([NullValidate("rpc.mer.config.id.null")] Guid id)
                {
                        return this._Service.GetConfig(id);
                }
                /// <summary>
                /// 获取配置列表
                /// </summary>
                /// <param name="rpcMerId"></param>
                /// <returns></returns>
                public RpcMerConfigDatum[] Gets([NumValidate("rpc.mer.id.null", 1)] long rpcMerId)
                {
                        return this._Service.GetConfigs(rpcMerId);
                }
                /// <summary>
                /// 修改配置
                /// </summary>
                /// <param name="id"></param>
                /// <param name="param"></param>
                public void Set([NullValidate("rpc.mer.config.id.null")] Guid id, SetMerConfig param)
                {
                        this._Service.Set(id, param);
                }
        }
}
