using SqlSugar;

namespace DataBasicCli.RpcExtendService
{
    [SugarTable("IdentityApp", "身份标识应用表")]
    public class IdentityAppModel
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "标识ID")]
        public long Id { get; set; }

        [SugarColumn(ColumnDescription = "身份标识AppId", ColumnDataType = "varchar", Length = 32)]
        public string AppId { get; set; }

        [SugarColumn(ColumnDescription = "身份标识名", Length = 50)]
        public string AppName { get; set; }
        [SugarColumn(ColumnDescription = "备注", Length = 255, IsNullable = true)]
        public string AppShow { get; set; }

        /// <summary>
        /// 应用扩展
        /// </summary>
        [SugarColumn(ColumnDescription = "扩展参数", IsNullable = true)]
        public string AppExtend { get; set; }

        [SugarColumn(ColumnDescription = "有效时间", ColumnDataType = "date")]
        public DateTime? EffectiveDate { get; set; }
        [SugarColumn(ColumnDescription = "是否启用")]
        public bool IsEnable { get; set; }
        [SugarColumn(ColumnDescription = "创建时间")]
        public DateTime CreateTime { get; set; }
    }
}
