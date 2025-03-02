using System;

namespace WeDonekRpc.Client.Queue.Model
{
    /// <summary>
    /// 队列链接
    /// </summary>
    public class QueueCon
    {
        /// <summary>
        /// 链接URI
        /// </summary>
        public Uri ServerUri { get; set; }

        /// <summary>
        /// 链接服务器节点
        /// </summary>
        public string ServerIp { get; set; }

        public string ToString (int defPort)
        {
            if (this.ServerUri != null)
            {
                return string.Concat(this.ServerUri.Authority, ":", this.ServerUri.IsDefaultPort ? defPort : this.ServerUri.Port);
            }
            else if (!this.ServerIp.Contains(':'))
            {
                return string.Concat(this.ServerIp, ":", defPort);
            }
            return this.ServerIp;
        }

    }
}
