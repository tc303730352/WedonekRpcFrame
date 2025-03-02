namespace RpcSync.Model.DB
{
    using System;
    using SqlSugar;
    using WeDonekRpc.Model;

    [SugarTable("BroadcastErrorLog")]
    public class BroadcastErrorLogModel
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }

        public string Error { get; set; }

        public string MsgKey { get; set; }

        public string MsgBody { get; set; }

        public long ServerId { get; set; }

        public string SysTypeVal { get; set; }

        /// <summary>
        /// 来源服务节点ID
        /// </summary>
        public long SourceId { get; set; }
        [SugarColumn(IsJson = true)]
        public MsgSource MsgSource { get; set; }
        public BroadcastType BroadcastType { get; set; }

        public string RouteKey { get; set; }

        public long RpcMerId { get; set; }
        public DateTime AddTime { get; set; }
    }
}
