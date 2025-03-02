using SqlSugar;

namespace RpcSync.Model.DB
{
    [SugarTable("KafkaRouteKey")]
    public class KafkaRouteKeyModel
    {
        [SugarColumn(IsPrimaryKey =true)]
        public long Id { get; set; } 

        public int ExchangeId { get; set; }
        /// <summary>
        /// 路由节点键
        /// </summary>
        public string RouteKey
        {
            get;
            set;
        }
        /// <summary>
        ///数据队列
        /// </summary>
        public string Queue
        {
            get;
            set;
        }
    }
}
