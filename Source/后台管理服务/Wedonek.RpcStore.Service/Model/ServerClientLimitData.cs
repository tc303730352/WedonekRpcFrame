using RpcModel.Model;

using RpcHelper.Validate;

namespace Wedonek.RpcStore.Service.Model
{
        /// <summary>
        /// 服务节点客户端限流配置
        /// </summary>
        public class ServerClientLimitData : ServerClientLimit
        {
                /// <summary>
                /// 集群Id
                /// </summary>
                [NumValidate("rpc.mer.id.error", 1)]
                public long RpcMerId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 节点Id
                /// </summary>
                [NumValidate("rpc.server.id.error", 1)]
                public long ServerId
                {
                        get;
                        set;
                }
        }
}
