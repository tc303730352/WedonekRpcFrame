using System;

namespace Wedonek.RpcStore.Service.Model
{
        /// <summary>
        /// 集群配置
        /// </summary>
        public class RpcMerConfig : AddMerConfig
        {
                /// <summary>
                /// 配置Id
                /// </summary>
                public Guid Id
                {
                        get;
                        set;
                }

        }
}
