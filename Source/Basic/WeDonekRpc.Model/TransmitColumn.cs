using System;

namespace WeDonekRpc.Model
{
    /// <summary>
    /// 负载均衡配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class TransmitColumn : Attribute
    {
        /// <summary>
        /// 负载均衡(负载均衡类型为ZoneIndex时有效)
        /// </summary>
        /// <param name="one">第一个字母位置</param>
        /// <param name="two">第二个字母位置</param>
        public TransmitColumn (int one, int two)
        {
            this.TransmitType = TransmitType.ZoneIndex;
            this.ZIndexBit = new int[] { one, two };
        }
        /// <summary>
        /// 负载均衡
        /// </summary>
        /// <param name="type">负载均衡类型</param>
        public TransmitColumn (TransmitType type)
        {
            this.TransmitType = type;
        }
        /// <summary>
        /// 负载均衡
        /// </summary>
        /// <param name="scheme">负载均衡的组</param>
        public TransmitColumn (string scheme)
        {
            this.Transmit = scheme;
        }
        /// <summary>
        /// 负载均衡方案
        /// </summary>
        public string Transmit
        {
            get;
            set;
        }
        /// <summary>
        /// 负载均衡类型
        /// </summary>
        public TransmitType TransmitType
        {
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        internal int[] ZIndexBit
        {
            get;
        }
    }
}
