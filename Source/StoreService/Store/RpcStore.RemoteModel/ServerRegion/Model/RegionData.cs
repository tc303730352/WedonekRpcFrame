namespace RpcStore.RemoteModel.ServerRegion.Model
{
    public class RegionData
    {
        /// <summary>
        /// 区域ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 区域名
        /// </summary>
        public string RegionName { get; set; }

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

        public string ProCity { get; set; }

        public int ServerNum { get; set; }
    }
}
