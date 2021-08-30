using System.Collections.Generic;

using RpcModel;

namespace AutoTaskService.Model
{
        /// <summary>
        /// 远程广播配置
        /// </summary>
        internal class RpcBroadcastConfig
        {
                /// <summary>
                /// 发送配置
                /// </summary>
                public RpcSendConfig SendConfig
                {
                        get;
                        set;
                }
                /// <summary>
                /// 远程服务节点地址
                /// </summary>
                public string[] TypeVal
                {
                        get;
                        set;
                }
                /// <summary>
                /// 节点列表
                /// </summary>
                public long[] ServerId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 是否限定唯一
                /// </summary>
                public bool IsOnly
                {
                        get;
                        set;
                }
                /// <summary>
                /// 是否排除来源
                /// </summary>
                public bool IsExclude
                {
                        get;
                        set;
                }
                /// <summary>
                /// 广播方式
                /// </summary>
                public BroadcastType BroadcastType
                {
                        get;
                        set;
                }
                /// <summary>
                /// 消息体
                /// </summary>
                public Dictionary<string, object> MsgBody { get; set; }
                /// <summary>
                /// 是否跨服务器组进行广播
                /// </summary>
                public bool IsCrossGroup { get; set; }

        }
}
