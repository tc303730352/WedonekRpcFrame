namespace RpcExtend.Model.DB
{
    using System;
    using SqlSugar;
    using WeDonekRpc.ExtendModel.SysError;

    [SugarTable("SystemErrorLog")]
    public class SystemErrorLog
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }

        public long RpcMerId { get; set; }

        [SugarColumn(DefaultValue = "", IsNullable = false)]
        public string TraceId { get; set; }

        public string LogTitle { get; set; }

        [SugarColumn(DefaultValue = "", IsNullable = false)]
        public string LogShow { get; set; }

        public string SystemType { get; set; }

        public long ServerId { get; set; }

        [SugarColumn(DefaultValue = "", IsNullable = false)]
        public string LogGroup { get; set; }

        public byte LogType { get; set; }

        public byte LogGrade { get; set; }

        [SugarColumn(DefaultValue = "", IsNullable = false)]
        public string ErrorCode { get; set; }

        [SugarColumn(IsJson = true)]
        public ExceptionMsg Exception { get; set; }

        [SugarColumn(DefaultValue = "", IsNullable = false)]
        public string AttrList { get; set; }

        public DateTime AddTime { get; set; }
    }
}
