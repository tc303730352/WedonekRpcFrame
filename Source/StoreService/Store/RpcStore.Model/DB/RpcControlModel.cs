using SqlSugar;

namespace RpcStore.Model.DB
{
    [SugarTable("RpcControlList")]
    public class RpcControlModel
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 服务中心链接的IP地址
        /// </summary>
        public string ServerIp
        {
            get;
            set;
        }
        /// <summary>
        /// 端口号
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 所在区域机房
        /// </summary>
        public int RegionId { get; set; }
        /// <summary>
        /// 服务中心说明
        /// </summary>
        public string Show { get; set; }
    }
}
