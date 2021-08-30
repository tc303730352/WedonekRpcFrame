using System;

namespace Wedonek.RpcStore.Service.Model
{
        /// <summary>
        /// 服务节点链接关系表
        /// </summary>
        public class ServerConRelation
        {
                /// <summary>
                /// 数据Id
                /// </summary>
                public Guid Id
                {
                        get;
                        set;
                }
                /// <summary>
                /// 服务Id
                /// </summary>
                public long ServerId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 链接Id
                /// </summary>
                public long ConServerId
                {
                        get;
                        set;
                }
        }
}
