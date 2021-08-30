using RpcHelper.Validate;
namespace Wedonek.RpcStore.Gateway.Model
{
        internal class BindServer
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
                /// 服务节点Id
                /// </summary>
                [NumValidate("rpc.server.id.error", 1)]
                public long ServerId
                {
                        get;
                        set;
                }
        }
}
