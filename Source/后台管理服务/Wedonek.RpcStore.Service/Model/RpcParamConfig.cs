using System.Collections.Generic;

using RpcHelper.Validate;
namespace Wedonek.RpcStore.Service.Model
{
        /// <summary>
        /// Rpc消息配置
        /// </summary>
        public class RpcParamConfig
        {
                /// <summary>
                /// 发送配置
                /// </summary>
                [NullValidate("rpc.task.send.config.null")]
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
