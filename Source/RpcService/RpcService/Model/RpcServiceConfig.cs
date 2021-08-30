using RpcModel;

namespace RpcService.Model
{
        /// <summary>
        /// 节点配置
        /// </summary>
        public class RpcServiceConfig : ServerConfig
        {
                /// <summary>
                /// 区域Id
                /// </summary>
                public int RegionId
                {
                        get;
                        set;
                }

        }
}
