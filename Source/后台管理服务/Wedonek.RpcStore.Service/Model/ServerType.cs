using RpcModel;

namespace Wedonek.RpcStore.Service.Model
{
        public class ServerType
        {
                /// <summary>
                /// 类型ID
                /// </summary>
                public long Id
                {
                        get;
                        set;
                }
                /// <summary>
                /// 服务组
                /// </summary>
                public long GroupId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 类型值
                /// </summary>
                public string TypeVal
                {
                        get;
                        set;
                }
                /// <summary>
                /// 服务名
                /// </summary>
                public string SystemName
                {
                        get;
                        set;
                }
                /// <summary>
                /// 负载均衡的方式
                /// </summary>
                public BalancedType BalancedType
                {
                        get;
                        set;
                }
        }
}
