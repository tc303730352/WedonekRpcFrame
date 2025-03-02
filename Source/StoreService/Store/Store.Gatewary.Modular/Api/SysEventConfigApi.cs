using WeDonekRpc.HttpApiGateway;
using RpcStore.RemoteModel.SysEventConfig.Model;
using Store.Gatewary.Modular.Interface;

namespace Store.Gatewary.Modular.Api
{
    /// <summary>
    /// 系统事件配置
    /// </summary>
    internal class SysEventConfigApi : ApiController
    {
        private readonly ISysEventConfigService _Service;

        public SysEventConfigApi (ISysEventConfigService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 获取配置详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SystemEventConfig Get (int id)
        {
            return this._Service.Get(id);
        }

        /// <summary>
        /// 获取事件配置项
        /// </summary>
        /// <returns></returns>
        public SystemEventItem[] GetItems ()
        {
            return this._Service.GetItems();
        }
    }
}
