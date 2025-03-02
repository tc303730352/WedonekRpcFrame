using SqlSugar;

namespace DataBasicCli.RpcService
{
    [SugarTable("RpcControlList", TableDescription = "服务中心信息表")]
    public class RpcControlModel
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnDescription = "服务中心ID")]
        public int Id { get; set; }

        /// <summary>
        /// 服务中心链接的IP地址
        /// </summary>
        [SugarColumn(ColumnDescription = "链接IP", ColumnDataType = "varchar", Length = 15)]
        public string ServerIp
        {
            get;
            set;
        }
        /// <summary>
        /// 端口号
        /// </summary>
        [SugarColumn(ColumnDescription = "端口号")]
        public int Port { get; set; }
        /// <summary>
        /// 所在区域机房
        /// </summary>
        [SugarColumn(ColumnDescription = "所在机房")]
        public int RegionId { get; set; }
        /// <summary>
        /// 服务中心说明
        /// </summary>
        [SugarColumn(ColumnDescription = "链接IP", Length = 255, IsNullable = true)]
        public string Show { get; set; }
    }
}
