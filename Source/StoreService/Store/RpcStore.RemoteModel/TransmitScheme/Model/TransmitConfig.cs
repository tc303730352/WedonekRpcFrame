using WeDonekRpc.Model;

namespace RpcStore.RemoteModel.TransmitScheme.Model
{
    /// <summary>
    /// 负载均衡配置明细
    /// </summary>
    public class TransmitConfig
    {
        /// <summary>
        /// 负载范围
        /// </summary>
        public TransmitRange[] Range
        {
            get;
            set;
        }
        /// <summary>
        /// 固定值
        /// </summary>
        public string Value { get; set; }
    }
}
