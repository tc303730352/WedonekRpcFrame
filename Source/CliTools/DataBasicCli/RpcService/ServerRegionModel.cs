using SqlSugar;

namespace DataBasicCli.RpcService
{
    [SugarTable("ServerRegion", TableDescription = "服务机房表")]
    public class ServerRegionModel
    {
        /// <summary>
        /// 区域Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnDescription = "机房ID")]
        public int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 区域名称
        /// </summary>
        [SugarColumn(ColumnDescription = "机房名", Length = 50, IsNullable = false)]
        public string RegionName
        {
            get;
            set;
        }
        /// <summary>
        /// 国家
        /// </summary>
        [SugarColumn(ColumnDescription = "国家ID")]
        public int CountryId { get; set; }
        /// <summary>
        /// 所在省份ID
        /// </summary>
        [SugarColumn(ColumnDescription = "所在省份ID")]
        public int ProId { get; set; }

        /// <summary>
        /// 城市ID
        /// </summary>
        [SugarColumn(ColumnDescription = "所在城市ID")]
        public int CityId { get; set; }

        /// <summary>
        /// 区县ID
        /// </summary>
        [SugarColumn(ColumnDescription = "所在区县ID", IsNullable = true)]
        public int? DistrictId { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [SugarColumn(ColumnDescription = "机房地址", Length = 100, IsNullable = true)]
        public string Address { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        [SugarColumn(ColumnDescription = "联系人", Length = 20, IsNullable = true)]
        public string Contacts { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        [SugarColumn(ColumnDescription = "联系电话", Length = 30, ColumnDataType = "varchar", IsNullable = true)]
        public string Phone { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        [SugarColumn(ColumnDescription = "是否删除")]
        public bool IsDrop
        {
            get;
            set;
        }
    }
}
