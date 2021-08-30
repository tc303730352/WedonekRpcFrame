using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Model
{
        /// <summary>
        /// 配置信息
        /// </summary>
        public class SysConfigInfo
        {
                /// <summary>
                /// 配置Id
                /// </summary>
                public long Id
                {
                        get;
                        set;
                }
                /// <summary>
                /// 配置范围
                /// </summary>
                public string Range
                {
                        get;
                        set;
                }

                /// <summary>
                /// 配置名
                /// </summary>
                public string Name
                {
                        get;
                        set;
                }
                /// <summary>
                /// 值类型
                /// </summary>
                public SysConfigValueType ValueType
                {
                        get;
                        set;
                }
                /// <summary>
                /// 值
                /// </summary>
                public string Value
                {
                        get;
                        set;
                }
        }
}
