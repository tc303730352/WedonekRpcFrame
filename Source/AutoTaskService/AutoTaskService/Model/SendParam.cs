namespace AutoTaskService.Model
{
        /// <summary>
        /// 发送配置
        /// </summary>
        internal class SendParam
        {
                /// <summary>
                /// Http配置
                /// </summary>
                public HttpParamConfig HttpConfig
                {
                        get;
                        set;
                }

                /// <summary>
                /// Rpc通信配置
                /// </summary>
                public RpcParamConfig RpcConfig
                {
                        get;
                        set;
                }

                /// <summary>
                /// 广播配置
                /// </summary>
                public RpcBroadcastConfig BroadcastConfig
                {
                        get;
                        set;
                }
        }
}
