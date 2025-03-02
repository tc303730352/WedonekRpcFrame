
using RpcSync.Service.Interface;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Model.Kafka;
using WeDonekRpc.Model.Kafka.Model;
namespace RpcSync.Service.Event
{
    /// <summary>
    /// Kafka 虚拟交换机
    /// </summary>
    internal class KafkaExchangeEvent : IRpcApiService
    {
        private readonly IKafkaExchangeService _Service;
        public KafkaExchangeEvent (IKafkaExchangeService service)
        {
            this._Service = service;
        }

        /// <summary>
        /// 注册交换机
        /// </summary>
        /// <param name="add"></param>
        public void AddKafkaExchange (AddKafkaExchange add)
        {
            this._Service.Sync(add.Exchange);
        }
        /// <summary>
        /// 获取交换机中的路由Key
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ExchangeRouteKey[] GetKafkaRouteKey (GetKafkaRouteKey obj)
        {
            return this._Service.GetRouteKeys(obj.Exchange);
        }
    }
}
