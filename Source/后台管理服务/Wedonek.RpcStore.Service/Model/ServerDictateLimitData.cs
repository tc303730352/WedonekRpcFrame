using RpcModel.Model;

namespace Wedonek.RpcStore.Service.Model
{
        /// <summary>
        /// 服务节点指令限流
        /// </summary>
        public class ServerDictateLimitData : ServerDictateLimit
        {
                /// <summary>
                /// 指令Id
                /// </summary>
                public long Id
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
