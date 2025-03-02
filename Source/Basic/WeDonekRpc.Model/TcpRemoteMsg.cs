using System.Collections.Generic;
using WeDonekRpc.Model.Model;
using WeDonekRpc.Model.Tran.Model;

namespace WeDonekRpc.Model
{
    public class TcpRemoteMsg
    {
        /// <summary>
        /// 包ID
        /// </summary>
        public uint PageId;
        /// <summary>
        /// 消息体
        /// </summary>
        public string MsgBody;
        /// <summary>
        /// 数据流
        /// </summary>
        public byte[] Stream;
        /// <summary>
        /// 消息来源
        /// </summary>
        public MsgSource Source;
        /// <summary>
        /// 扩展参数
        /// </summary>
        public Dictionary<string, string> Extend;
        /// <summary>
        /// 是否加锁
        /// </summary>
        public bool IsSync;
        /// <summary>
        /// 锁ID
        /// </summary>
        public string LockId;
        /// <summary>
        /// 是否需要服务器回复
        /// </summary>
        public bool IsReply;
        /// <summary>
        /// 锁类型
        /// </summary>
        public RemoteLockType LockType;
        /// <summary>
        /// 消息过期时间
        /// </summary>
        public int ExpireTime;
        /// <summary>
        /// 事务状态
        /// </summary>
        public CurTranState Tran;
        /// <summary>
        /// 链路
        /// </summary>
        public TrackSpan Track;

        /// <summary>
        /// 当前重试数
        /// </summary>
        public int Retry { get; set; }
    }
}
