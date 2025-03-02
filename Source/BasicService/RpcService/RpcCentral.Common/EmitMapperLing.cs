using RpcCentral.Common;
using RpcCentral.Common.EmitMapper;
using WeDonekRpc.Helper;

namespace RpcCentral.Common
{
    /// <summary>
    /// 实体转换的Ling扩展
    /// </summary>
    public static class EmitMapperLing
    {
        /// <summary>
        /// 成员类型转换
        /// </summary>
        /// <typeparam name="T">转换的对象</typeparam>
        /// <typeparam name="Result">转换结果</typeparam>
        /// <param name="data">转换的实例</param>
        /// <returns>转换后的实例</returns>
        public static Result ConvertMap<T, Result> (this T data) where T : class
        {
            return EmitMapperCollect.Map<T, Result>(data);
        }
        public static Result ConvertMap<T, Result> (this T data, Func<T, Result, Result> func) where T : class
        {
            return func(data, EmitMapperCollect.Map<T, Result>(data));
        }
        /// <summary>
        /// 成员类型转换
        /// </summary>
        /// <typeparam name="T">转换对象</typeparam>
        /// <typeparam name="Result">转换结果</typeparam>
        /// <param name="data">转换的实例数组</param>
        /// <returns>转换后的实例数组</returns>
        public static Result[] ConvertMap<T, Result> (this T[] data) where T : class
        {
            return EmitMapperCollect.Map<T, Result>(data);
        }
        public static Result[] ConvertMap<T, Result> (this T[] data, Func<T, Result, Result> func) where T : class
        {
            return data.ConvertAll(a => a.ConvertMap<T, Result>(func));
        }
        public static Result[] ConvertMap<T, Result> (this T[] data, Predicate<T> match) where T : class
        {
            return EmitMapperCollect.Map<T, Result>(data.FindAll(match));
        }
        public static Result[] ConvertMap<T, Result> (this T[] data, Predicate<T> match, Func<T, Result, Result> func) where T : class
        {
            return data.Convert(c =>
            {
                if (!match(c))
                {
                    return default;
                }
                return c.ConvertMap<T, Result>(func);
            });
        }
        /// <summary>
        /// 成员类型转换
        /// </summary>
        /// <typeparam name="T">转换对象</typeparam>
        /// <typeparam name="Result">转换结果</typeparam>
        /// <param name="data">转换的实例集合</param>
        /// <returns>转换后的实例数组</returns>
        public static Result[] ConvertMap<T, Result> (this IEnumerable<T> data) where T : class
        {
            return EmitMapperCollect.Map<T, Result>(data.ToArray());
        }
        /// <summary>
        /// 将指定对象属性值覆盖现有值
        /// </summary>
        /// <typeparam name="T">原对象</typeparam>
        /// <typeparam name="Result">覆盖对象</typeparam>
        /// <param name="result">覆盖的实例</param>
        /// <param name="data">原实例</param>
        /// <returns>覆盖后的实例</returns>
        public static Result Into<T, Result> (this Result result, T data) where T : class
        {
            return EmitMapperCollect.Map(data, result);
        }
        /// <summary>
        /// 将指定对象属性值覆盖现有值
        /// </summary>
        /// <typeparam name="T">原对象</typeparam>
        /// <typeparam name="Result">覆盖对象</typeparam>
        /// <param name="result">覆盖的实例</param>
        /// <param name="data">原实例</param>
        /// <returns>覆盖后的实例</returns>
        public static Result[] Into<T, Result> (this Result[] result, T[] data) where T : class
        {
            return EmitMapperCollect.Map(data, result);
        }


    }
}
