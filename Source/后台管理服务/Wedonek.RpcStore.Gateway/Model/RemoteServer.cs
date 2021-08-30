using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Model
{
        /// <summary>
        /// 服务节点
        /// </summary>
        public class RemoteServer : ServerConfigDatum
        {
                /// <summary>
                /// 所属类别
                /// </summary>
                public string SystemName
                {
                        get;
                        set;
                }
                /// <summary>
                /// 负载方式
                /// </summary>
                public string BalancedType
                {
                        get;
                        set;
                }
        }
}
