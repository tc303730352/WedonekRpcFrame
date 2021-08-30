using System;

namespace RpcClient.Queue.Model
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

        }
}
