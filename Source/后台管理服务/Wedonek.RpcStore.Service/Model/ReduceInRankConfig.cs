namespace Wedonek.RpcStore.Service.Model
{
        public class ReduceInRankConfig : ReduceInRankDatum
        {
                /// <summary>
                /// 数据Id
                /// </summary>
                public long Id
                {
                        get;
                        set;
                }
                /// <summary>
                /// 数据Id
                /// </summary>
                public long RpcMerId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 服务Id
                /// </summary>
                public long ServerId
                {
                        get;
                        set;
                }

        }
}
