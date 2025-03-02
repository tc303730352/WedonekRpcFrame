namespace WeDonekRpc.Model.Kafka.Model
{
    public class ExchangeDatum
    {
        public string Exchange
        {
            get;
            set;
        }

        public ExchangeRouteKey[] RouteKey
        {
            get;
            set;
        }
    }
}
