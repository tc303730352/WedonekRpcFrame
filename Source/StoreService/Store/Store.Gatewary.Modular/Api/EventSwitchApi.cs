using RpcStore.RemoteModel.ServerEventSwitch.Model;
using Store.Gatewary.Modular.Interface;
using WeDonekRpc.ApiGateway.Attr;
using WeDonekRpc.Client;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.HttpApiGateway;
using WeDonekRpc.HttpApiGateway.Model;

namespace Store.Gatewary.Modular.Api
{
    /// <summary>
    /// 服务事件
    /// </summary>
    internal class EventSwitchApi : ApiController
    {
        private readonly IEventSwitchService _Service;

        public EventSwitchApi (IEventSwitchService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 添加服务事件
        /// </summary>
        /// <param name="datum"></param>
        /// <returns></returns>
        [ApiPrower("rpc.store.admin")]
        public long Add ([NullValidate("rpc.store.event.data.null")] EventSwitchAdd datum)
        {
            return this._Service.Add(datum);
        }
        /// <summary>
        /// 删除服务事件
        /// </summary>
        /// <param name="id"></param>
        [ApiPrower("rpc.store.admin")]
        public void Delete ([NumValidate("rpc.store.event.id.error", 1)] long id)
        {
            this._Service.Delete(id);
        }
        /// <summary>
        /// 获取服务事件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EventSwitchData Get ([NumValidate("rpc.store.event.id.error", 1)] long id)
        {
            return this._Service.Get(id);
        }
        /// <summary>
        /// 查询服务事件
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public PagingResult<EventSwitch> Query (PagingParam<EventSwitchQuery> param)
        {
            return this._Service.Query(param);
        }
        /// <summary>
        /// 修改服务事件
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        [ApiPrower("rpc.store.admin")]
        public bool Update (LongParam<EventSwitchSet> set)
        {
            return this._Service.Update(set.Id, set.Value);
        }
        /// <summary>
        /// 设置是否启用
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        [ApiPrower("rpc.store.admin")]
        public void SetIsEnable (LongParam<bool> set)
        {
            this._Service.SetIsEnable(set.Id, set.Value);
        }
    }
}
