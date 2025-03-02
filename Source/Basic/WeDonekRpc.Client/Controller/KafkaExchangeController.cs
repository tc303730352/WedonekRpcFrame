
using WeDonekRpc.Client.Collect;

using WeDonekRpc.Helper;

using WeDonekRpc.Model.Kafka;
using WeDonekRpc.Model.Kafka.Model;

namespace WeDonekRpc.Client.Controller
{
    /// <summary>
    /// Kafka交换机
    /// </summary>
    internal class KafkaExchangeController : DataSyncClass
    {
        public KafkaExchangeController(string exchange)
        {
            this.Exchange = exchange;
        }
        /// <summary>
        /// 交换机名称
        /// </summary>
        public string Exchange
        {
            get;
        }
        /// <summary>
        /// 交换机路由Key
        /// </summary>
        private ExchangeRouteKey[] _RouteKeys;
        protected override void SyncData()
        {
            if (!RemoteCollect.Send(new GetKafkaRouteKey
            {
                Exchange = this.Exchange
            }, out ExchangeRouteKey[] list, out string error))
            {
                throw new ErrorException(error);
            }
            _RouteKeys = list;
        }
        public string[] GetQueue(string[] routeKey)
        {
            return _RouteKeys.Distinct(a => routeKey.IsExists(a.RouteKey), a => a.Queue);
        }
    }
}
