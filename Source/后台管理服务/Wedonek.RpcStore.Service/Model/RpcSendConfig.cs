using RpcModel;

using RpcHelper.Validate;

namespace Wedonek.RpcStore.Service.Model
{
        /// <summary>
        /// 发送配置
        /// </summary>
        public class RpcSendConfig
        {
                /// <summary>
                /// 指令
                /// </summary>
                [NullValidate("rpc.task.rpc.dictate.null")]
                public string SysDictate
                {
                        get;
                        set;
                }

                /// <summary>
                /// 失败是否重试
                /// </summary>
                public bool IsRetry
                {
                        get;
                        set;
                }
                /// <summary>
                /// 负载均衡ID
                /// </summary>
                public int TransmitId { get; set; }
                /// <summary>
                /// 负载方式
                /// </summary>
                public TransmitType TransmitType
                {
                        get;
                        set;
                }

                /// <summary>
                /// 负载计算列
                /// </summary>
                public string IdentityColumn
                {
                        get;
                        set;
                }
                /// <summary>
                /// 计算ZIndex的值
                /// </summary>
                public int[] ZIndexBit
                {
                        get;
                        private set;
                }
                /// <summary>
                /// 锁类型
                /// </summary>
                public RemoteLockType LockType
                {
                        get;
                        set;
                }
                /// <summary>
                /// 锁列
                /// </summary>
                public string[] LockColumn
                {
                        get;
                        set;
                }
        }
}
