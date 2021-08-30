using System.Collections.Generic;

using RpcModel.Model;
using RpcModel.Tran.Model;

namespace RpcModel
{
        public class TcpRemoteMsg
        {
                /// <summary>
                /// 消息体
                /// </summary>
                public string MsgBody
                {
                        get;
                        set;
                }
                /// <summary>
                /// 消息来源
                /// </summary>
                public MsgSource Source { get; set; }
                /// <summary>
                /// 扩展参数
                /// </summary>
                public Dictionary<string, string> Extend { get; set; }
                /// <summary>
                /// 是否加锁
                /// </summary>
                public bool IsSync { get; set; }
                /// <summary>
                /// 锁ID
                /// </summary>
                public string LockId { get; set; }
                /// <summary>
                /// 是否需要服务器回复
                /// </summary>
                public bool IsReply { get; set; }
                /// <summary>
                /// 锁类型
                /// </summary>
                public RemoteLockType LockType { get; set; }
                /// <summary>
                /// 消息过期时间
                /// </summary>
                public int ExpireTime { get; set; }
                /// <summary>
                /// 事务状态
                /// </summary>
                public CurTranState Tran { get; set; }
                /// <summary>
                /// 链路
                /// </summary>
                public TrackSpan Track { get; set; }
        }
}
