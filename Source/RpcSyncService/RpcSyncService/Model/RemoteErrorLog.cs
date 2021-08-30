using System;

namespace RpcSyncService.Model
{
        internal class RemoteErrorLog
        {
                public Guid Id { get; set; }
                public long ErrorCode { get; set; }
                public string MsgKey { get; set; }
                public string MsgBody { get; set; }
                public long ServerId { get; set; }
                public string SysTypeVal { get; set; }
                public string MsgSource { get; set; }
                public long RpcMerId { get; set; }
                public DateTime AddTime { get; set; }
        }
}
