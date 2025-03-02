using System;

namespace WeDonekRpc.Model.Kafka.Model
{
    /// <summary>
    /// 交换机路由
    /// </summary>
    public class ExchangeRouteKey : IEquatable<ExchangeRouteKey>
    {
        /// <summary>
        /// 路由节点键
        /// </summary>
        public string RouteKey
        {
            get;
            set;
        }
        /// <summary>
        ///数据队列
        /// </summary>
        public string Queue
        {
            get;
            set;
        }
        public override bool Equals(object obj)
        {
            if (obj is ExchangeRouteKey i)
            {
                return i.RouteKey == this.RouteKey && this.Queue == i.Queue;
            }
            return false;
        }

        public bool Equals(ExchangeRouteKey other)
        {
            if (other == null)
            {
                return false;
            }
            return other.RouteKey == this.RouteKey && this.Queue == other.Queue;
        }

        public override int GetHashCode()
        {
            return string.Concat(this.RouteKey, "_", this.Queue).GetHashCode();
        }
    }
}
