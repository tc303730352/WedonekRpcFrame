using HttpApiGateway.Model;

using RpcHelper.Validate;

using Wedonek.RpcStore.Gateway.Interface;
using Wedonek.RpcStore.Gateway.Model;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Controller
{
        /// <summary>
        /// 系统配置
        /// </summary>
        internal class SysConfigController : HttpApiGateway.ApiController
        {
                private readonly ISysConfigService _Config = null;
                public SysConfigController(ISysConfigService config)
                {
                        this._Config = config;
                }
                /// <summary>
                /// 添加配置
                /// </summary>
                /// <param name="add">配置信息</param>
                /// <returns>配置Id</returns>
                public long Add(SysConfigAddParam add)
                {
                        return this._Config.Add(add);
                }
                /// <summary>
                /// 删除配置
                /// </summary>
                /// <param name="configId">配置Id</param>
                public void Drop([NullValidate("rpc.config.id.error")] long configId)
                {
                        this._Config.Drop(configId);
                }
                /// <summary>
                /// 获取配置信息
                /// </summary>
                /// <param name="configId">配置Id</param>
                /// <returns>配置信息</returns>
                public SysConfigDatum Get([NullValidate("rpc.config.id.error")] long configId)
                {
                        return this._Config.Get(configId);
                }
                /// <summary>
                /// 配置查询
                /// </summary>
                /// <param name="query">查询参数</param>
                /// <param name="count">数据总量</param>
                /// <returns>配置资料</returns>
                public SysConfigInfo[] Query(PagingParam<QuerySysParam> query, out long count)
                {
                        return this._Config.Query(query, out count);
                }
                /// <summary>
                /// 设置配置
                /// </summary>
                /// <param name="config">配置</param>
                public void Set(SysConfigSet config)
                {
                        this._Config.Set(config);
                }
        }
}
