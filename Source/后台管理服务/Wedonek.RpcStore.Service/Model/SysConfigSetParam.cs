using System;

namespace Wedonek.RpcStore.Service.Model
{
        public class SysConfigSetParam
        {
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


                /// <summary>
                /// 更新时间
                /// </summary>
                public DateTime ToUpdateTime
                {
                        get;
                        set;
                }
        }
}
