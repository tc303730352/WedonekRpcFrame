using RpcModel.Model;

using RpcHelper.Validate;

namespace Wedonek.RpcStore.Service.Model
{
        /// <summary>
        /// 添加节点限制配置
        /// </summary>
        public class AddServerLimitConfig : ServerLimitConfig
        {
                /// <summary>
                /// 服务节点Id
                /// </summary>
                [NumValidate("rpc.server.id.error", 1)]
                public long ServerId { get; set; }
        }
}
