using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Validate;
namespace RpcStore.RemoteModel.ServerRegion.Model
{
    public class RegionDatum
    {
        /// <summary>
        /// 机房名称
        /// </summary>
        [NullValidate("rpc.store.region.name.null")]
        [LenValidate("rpc.store.region.name.len", 2, 50)]
        public string RegionName { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        [AreaValidate("rpc.store.country.id.error", AreaType.国家, isAllowNull: true)]
        public int CountryId { get; set; }
        /// <summary>
        /// 所在省份ID
        /// </summary>
        [AreaValidate("rpc.store.pro.id.error", "CountryId")]
        public int ProId { get; set; }

        /// <summary>
        /// 城市ID
        /// </summary>
        [AreaValidate("rpc.store.city.id.error", "ProId")]
        public int CityId { get; set; }

        /// <summary>
        /// 区县ID
        /// </summary>
        [AreaValidate("rpc.store.districtId.id.error", "CityId")]
        public int? DistrictId { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [LenValidate("rpc.store.address.len", 0, 100)]
        public string Address { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        [LenValidate("rpc.store.contact.len", 0, 50)]
        public string Contacts { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        [LenValidate("rpc.store.phone.len", 0, 36)]
        [FormatValidate("rpc.store.phone.error", ValidateFormat.联系电话)]
        public string Phone { get; set; }
    }
}
