using RpcHelper.Validate;

namespace Wedonek.RpcStore.Service.Model
{
        public class AddReduceInRank : ReduceInRankDatum
        {
                /// <summary>
                /// 数据Id
                /// </summary>
                [NumValidate("rpc.mer.id.null", 1)]
                public long RpcMerId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 服务Id
                /// </summary>
                [NumValidate("rpc.server.id.null", 1)]
                public long ServerId
                {
                        get;
                        set;
                }
        }
}
