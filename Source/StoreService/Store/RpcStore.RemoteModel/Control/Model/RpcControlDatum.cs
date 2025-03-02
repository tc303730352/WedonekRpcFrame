using WeDonekRpc.Helper.Validate;

namespace RpcStore.RemoteModel.Control.Model
{
    /// <summary>
    /// 服务中心设置
    /// </summary>
    public class RpcControlDatum
    {
        /// <summary>
        /// 服务中心链接的IP地址
        /// </summary>
        [NullValidate("rpc.store.control.server.ip.null")]
        [FormatValidate("rpc.store.control.server.ip.error", ValidateFormat.IP)]
        public string ServerIp
        {
            get;
            set;
        }
        /// <summary>
        /// 端口号
        /// </summary>
        [NumValidate("rpc.store.control.port.error", 1)]
        public int Port { get; set; }
        /// <summary>
        /// 所在区域机房
        /// </summary>
        [NumValidate("rpc.store.region.id.error", 1, int.MaxValue)]
        public int RegionId { get; set; }
        /// <summary>
        /// 服务中心说明
        /// </summary>
        [LenValidate("rpc.store.control.show.len", 0, 255)]
        public string Show { get; set; }
    }
}
