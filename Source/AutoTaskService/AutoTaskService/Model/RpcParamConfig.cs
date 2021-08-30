using System.Collections.Generic;
namespace AutoTaskService.Model
{
        internal class RpcParamConfig
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
                /// 服务器类型
                /// </summary>
                public string SystemType
                {
                        get;
                        set;
                }
                /// <summary>
                /// 服务节点Id
                /// </summary>
                public long ServerId
                {
                        get;
                        set;
                }

                /// <summary>
                /// 消息体
                /// </summary>
                public Dictionary<string, object> MsgBody { get; set; }

        }
}
