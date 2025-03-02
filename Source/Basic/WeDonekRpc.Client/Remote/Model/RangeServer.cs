using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Remote.Model
{
    /// <summary>
    /// 节点转发配置
    /// </summary>
    internal class RangeServer
    {
        /// <summary>
        /// 节点Id
        /// </summary>
        public long ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 转发类型
        /// </summary>
        public TransmitType TransmitType
        {
            get;
            set;
        }
        /// <summary>
        /// 限定值
        /// </summary>
        public string Value
        {
            get;
            set;
        }
        /// <summary>
        /// 限定范围
        /// </summary>
        public TransmitRange[] Range
        {
            get;
            set;
        }
        /// <summary>
        /// 方案
        /// </summary>
        public string Scheme { get; set; }
    }
}
