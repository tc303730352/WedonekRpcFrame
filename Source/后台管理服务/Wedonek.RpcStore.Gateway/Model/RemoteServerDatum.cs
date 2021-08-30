using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Model
{
        /// <summary>
        /// 远程服务资料
        /// </summary>
        public class RemoteServerDatum : RemoteServer
        {
                /// <summary>
                /// 组别名称
                /// </summary>
                public string GroupName
                {
                        get;
                        set;
                }

                /// <summary>
                /// 分发配置
                /// </summary>
                public TransmitConfig[] TransmitConfig
                {
                        get;
                        set;
                }
        }
}
