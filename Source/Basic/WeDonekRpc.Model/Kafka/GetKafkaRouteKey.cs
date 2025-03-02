namespace WeDonekRpc.Model.Kafka
{
        [IRemoteConfig("GetKafkaRouteKey", "sys.sync")]
        public  class GetKafkaRouteKey
        {
                public string Exchange
                {
                        get;
                        set;
                }
        }
}
