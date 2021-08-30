using RpcHelper.Validate;
namespace Wedonek.RpcStore.Service.Model
{
        /// <summary>
        /// 新建配置
        /// </summary>
        public class AddMerConfig : SetMerConfig
        {
                /// <summary>
                /// 集群Id
                /// </summary>
                [NumValidate("rpc.mer.id.null", 1)]
                public long RpcMerId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 节点类型
                /// </summary>
                [NumValidate("rpc.systemType.id.null", 1)]
                public long SystemTypeId
                {
                        get;
                        set;
                }

        }
}
