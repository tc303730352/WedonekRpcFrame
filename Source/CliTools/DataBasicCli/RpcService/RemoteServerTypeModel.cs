using SqlSugar;

namespace DataBasicCli.RpcService
{
    [SugarIndex("IX_GroupId", "GroupId", OrderByType.Asc, false)]
    [SugarIndex("IX_TypeVal", "TypeVal", OrderByType.Asc, true)]
    [SugarTable("RemoteServerType", TableDescription = "服务类别定义表")]
    public class RemoteServerTypeModel
    {
        /// <summary>
        /// 标识ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "标识ID")]
        public long Id
        {
            get;
            set;
        }
        [SugarColumn(ColumnDescription = "所属服务组别ID")]
        public long GroupId
        {
            get;
            set;
        }
        /// <summary>
        /// 类型值
        /// </summary>
        [SugarColumn(ColumnDescription = "类别值", Length = 50, ColumnDataType = "varchar")]
        public string TypeVal
        {
            get;
            set;
        }
        /// <summary>
        /// 服务名
        /// </summary>
        [SugarColumn(ColumnDescription = "类别名", Length = 50)]
        public string SystemName
        {
            get;
            set;
        }
        /// <summary>
        /// 默认端口
        /// </summary>
        [SugarColumn(ColumnDescription = "默认端口号")]
        public int DefPort
        {
            get;
            set;
        }

        /// <summary>
        /// 服务类型
        /// </summary>
        [SugarColumn(ColumnDescription = "默认服务类型")]
        public byte ServiceType
        {
            get;
            set;
        }
        /// <summary>
        /// 添加时间
        /// </summary>
        [SugarColumn(ColumnDescription = "添加时间")]
        public DateTime AddTime
        {
            get;
            set;
        }
    }
}
