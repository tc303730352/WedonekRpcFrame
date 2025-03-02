using System;

namespace WeDonekRpc.Client.Route
{
    /// <summary>
    /// 方法参数类型
    /// </summary>
    internal enum FuncParamType
    {
        参数 = 0,
        数据源 = 1,
        返回值 = 2,
        源 = 3,
        扩展参数 = 4,
        接口 = 5,
        数据流 = 6
    }
    /// <summary>
    /// 方法参数
    /// </summary>
    internal class FuncParam
    {
        /// <summary>
        /// 列名
        /// </summary>
        public string Key;
        /// <summary>
        /// 类型
        /// </summary>
        public FuncParamType ParamType;
        /// <summary>
        /// 数据类型
        /// </summary>
        public Type DataType;
    }
}
