using RpcHelper.Validate;

namespace Wedonek.RpcStore.Gateway.Model
{
        internal class DictateQueryParam
        {
                /// <summary>
                /// 服务ID
                /// </summary>
                [NumValidate("rpc.server.id.error", 1)]
                public long ServerId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 指令
                /// </summary>
                public string Dictate
                {
                        get;
                        set;
                }
        }
}
