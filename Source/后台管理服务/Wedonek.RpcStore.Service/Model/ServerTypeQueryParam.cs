using RpcModel;

namespace Wedonek.RpcStore.Service.Model
{
        public class ServerTypeQueryParam
        {
                /// <summary>
                /// 所属组别
                /// </summary>
                public long GroupId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 类型名
                /// </summary>
                public string Name
                {
                        get;
                        set;
                }

                /// <summary>
                /// 负载均衡的方式
                /// </summary>
                public BalancedType? BalancedType
                {
                        get;
                        set;
                }
        }
}
