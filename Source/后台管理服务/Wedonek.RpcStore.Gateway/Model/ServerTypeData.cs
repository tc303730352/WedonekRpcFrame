namespace Wedonek.RpcStore.Gateway.Model
{
        /// <summary>
        /// 服务类型资料
        /// </summary>
        public class ServerTypeData
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
                public string GroupName
                {
                        get;
                        set;
                }
                /// <summary>
                /// 类型值
                /// </summary>
                public string GroupVal
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
                public string BalancedType
                {
                        get;
                        set;
                }
        }
}
