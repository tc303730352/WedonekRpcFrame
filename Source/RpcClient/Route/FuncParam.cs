using System;

namespace RpcClient.Route
{
        /// <summary>
        /// 方法参数类型
        /// </summary>
        internal enum FuncParamType
        {
                参数 = 0,
                数据源 = 1,
                返回值 = 2,
                错误信息 = 3,
                源 = 4,
                数据总数 = 5,
                扩展参数 = 6,
                接口 = 7
        }
        /// <summary>
        /// 方法参数
        /// </summary>
        internal class FuncParam
        {
                public string Key { get; set; }
                /// <summary>
                /// 类型
                /// </summary>
                public FuncParamType ParamType
                {
                        get;
                        set;
                }
                /// <summary>
                /// 数据类型
                /// </summary>
                public Type DataType
                {
                        get;
                        set;
                }
        }
}
