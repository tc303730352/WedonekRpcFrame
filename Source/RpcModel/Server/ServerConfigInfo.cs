using RpcModel.Model;

namespace RpcModel
{
        public class ServerConfigInfo : RpcServerConfig
        {
                /// <summary>
                /// 降级配置
                /// </summary>
                public ReduceInRank Reduce
                {
                        get;
                        set;
                }
                /// <summary>
                /// 限流配置
                /// </summary>
                public ServerClientLimit ClientLimit
                {
                        get;
                        set;
                }
        }
}
