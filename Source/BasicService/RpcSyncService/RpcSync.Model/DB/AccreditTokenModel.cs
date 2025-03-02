namespace RpcSync.Model.DB
{
    using System;
    using SqlSugar;

    [SugarTable("AccreditToken")]
    public class AccreditTokenModel
    {
        [SugarColumn(IsPrimaryKey =true)]
        public string AccreditId { get; set; }

        public string PAccreditId { get; set; }
        [SugarColumn(IsJson =true,ColumnDataType ="varchar(1000)")]
        public string[] AccreditRole { get; set; }

        public string ApplyId { get; set; }

        public string CheckKey { get; set; }

        public string RoleType { get; set; }

        public string State { get; set; }

        public int StateVer { get; set; }

        public long RpcMerId { get; set; }

        public string SysGroup { get; set; }

        public string SystemType { get; set; }

        public DateTime? Expire { get; set; }

        public DateTime OverTime { get; set; }

        public DateTime AddTime { get; set; }
    }
}
