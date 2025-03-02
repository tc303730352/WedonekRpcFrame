using SqlSugar;

namespace RpcStore.Model.DB
{
    [SugarTable("ServerRegion")]
    public class ServerRegionModel
    {
        /// <summary>
        /// 区域Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string RegionName
        {
            get;
            set;
        }
        /// <summary>
        /// 国家
        /// </summary>
        public int CountryId { get; set; }
        /// <summary>
        /// 所在省份ID
        /// </summary>
        public int ProId { get; set; }

        /// <summary>
        /// 城市ID
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// 区县ID
        /// </summary>
        public int? DistrictId { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string Contacts { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDrop
        {
            get;
            set;
        }
    }
}
