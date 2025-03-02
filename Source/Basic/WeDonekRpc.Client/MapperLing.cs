using System;
using System.Collections.Generic;
using System.Linq;

using WeDonekRpc.Client.Interface;

using WeDonekRpc.Helper;
namespace WeDonekRpc.Client
{
    public delegate bool IsTreeNode<T> (T node, T parent);
    /// <summary>
    /// 实体结构转换
    /// </summary>
    public static class MapperLing
    {
        private static readonly IMapperCollect _Mapper = null;

        static MapperLing ()
        {
            _Mapper = RpcClient.Ioc.Resolve<IMapperCollect>();
        }

        /// <summary>
        /// 集合实体转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Result"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Result[] ConvertMap<T, Result> (this IEnumerable<T> data) where T : class
        {
            return _Mapper.Mapper<T, Result>(data.ToArray());
        }
        public static Result[] ConvertMap<T, Result> (this IEnumerable<T> data, Func<T, Result, Result> func) where T : class
        {
            return data.ToArray().ConvertAll(a =>
              {
                  Result to = _Mapper.Mapper<T, Result>(a);
                  return func(a, to);
              });
        }
        public static To ConvertMap<From, To> (this From from) where From : class
        {
            return _Mapper.Mapper<From, To>(from);
        }
        public static To ConvertMap<From, To> (this From from, Func<From, To, To> func) where From : class
        {
            return func(from, _Mapper.Mapper<From, To>(from));
        }
        public static To[] ConvertMap<From, To> (this From[] from) where From : class
        {
            return _Mapper.Mapper<From[], To[]>(from);
        }
        public static List<To> ConvertMapToList<From, To> (this From[] from) where From : class
        {
            return _Mapper.Mapper<From[], List<To>>(from);
        }
        public static To[] ConvertMap<From, To> (this From[] from, Func<From, To, To> func) where From : class
        {
            return from.ConvertAll(a =>
             {
                 To to = _Mapper.Mapper<From, To>(a);
                 return func(a, to);
             });
        }
        public static To[] ConvertMap<From, To> (this From[] from, Predicate<From> match, Func<From, To, To> func) where From : class
        {
            return from.Convert(match, a =>
              {
                  To to = _Mapper.Mapper<From, To>(a);
                  return func(a, to);
              });
        }
        public static To[] ConvertTree<From, To> (this From[] from, Predicate<From> isRoot, Func<From, From, bool> isNode) where From : class where To : IMapperTree<To>
        {
            return from.Convert(isRoot, a =>
            {
                return _ConvertTree<From, To>(from, a, isNode);
            });
        }
        private static To _ConvertTree<From, To> (From[] from, From parent, Func<From, From, bool> match) where From : class where To : IMapperTree<To>
        {
            To to = _Mapper.Mapper<From, To>(parent);
            to.Children = from.Convert(a => match(a, parent), a =>
             {
                 return _ConvertTree<From, To>(from, a, match);
             });
            return to;
        }
        public static To[] ConvertMap<From, To> (this From[] from, Predicate<From> match) where From : class
        {
            return _Mapper.Mapper<From[], To[]>(from.FindAll(match));
        }
        public static To[] ConvertMapToArray<From, To> (this List<From> from) where From : class
        {
            return _Mapper.Mapper<List<From>, To[]>(from);
        }
        public static To[] ConvertMapToArray<From, To> (this List<From> from, Func<From, To, To> func) where From : class
        {
            return from.ConvertAllToArray<From, To>(a => func(a, _Mapper.Mapper<From, To>(a)));
        }
        public static List<To> ConvertMap<From, To> (this List<From> from) where From : class
        {
            return _Mapper.Mapper<List<From>, List<To>>(from);
        }
        public static List<To> ConvertMap<From, To> (this List<From> from, Predicate<From> match) where From : class
        {
            return _Mapper.Mapper<List<From>, List<To>>(from.FindAll(match));
        }
        public static To ConvertFind<From, To> (this From[] from, Predicate<From> match) where From : class
        {
            return _Mapper.Mapper<From, To>(from.Find(match));
        }
        public static To ConvertFind<From, To> (this List<From> from, Predicate<From> match) where From : class
        {
            return _Mapper.Mapper<From, To>(from.Find(match));
        }

        public static To ConvertMap<From, To> (this From from, IMapperConfig config) where From : class
        {
            return _Mapper.Mapper<From, To>(from, config);
        }
        public static To[] ConvertMap<From, To> (this From[] from, IMapperConfig config) where From : class
        {
            return _Mapper.Mapper<From[], To[]>(from, config);
        }
        public static To[] ConvertMap<From, To> (this From[] from, Predicate<From> match, IMapperConfig config) where From : class
        {
            return _Mapper.Mapper<From[], To[]>(from.FindAll(match), config);
        }

        public static List<To> ConvertMap<From, To> (this List<From> from, IMapperConfig config) where From : class
        {
            return _Mapper.Mapper<List<From>, List<To>>(from, config);
        }
        public static List<To> ConvertMap<From, To> (this List<From> from, Predicate<From> match, IMapperConfig config) where From : class
        {
            return _Mapper.Mapper<List<From>, List<To>>(from.FindAll(match), config);
        }

        public static To ConvertFind<From, To> (this From[] from, Predicate<From> match, IMapperConfig config) where From : class
        {
            return _Mapper.Mapper<From, To>(from.Find(match), config);
        }

        public static To ConvertFind<From, To> (this List<From> from, Predicate<From> match, IMapperConfig config) where From : class
        {
            return _Mapper.Mapper<From, To>(from.Find(match), config);
        }
        /// <summary>
        /// 根据输入实体值覆盖输出实体值
        /// </summary>
        /// <typeparam name="T">输入</typeparam>
        /// <typeparam name="Result">输出</typeparam>
        /// <param name="result">输出实体</param>
        /// <param name="data">输入实体</param>
        /// <returns></returns>
        public static Result ConvertInto<T, Result> (this Result result, T data) where T : class
        {
            return _Mapper.Mapper(data, result);
        }
        /// <summary>
        /// 根据输入实体数组设置输出实体数组
        /// </summary>
        /// <typeparam name="T">输入</typeparam>
        /// <typeparam name="Result">输出</typeparam>
        /// <param name="result">输出实体</param>
        /// <param name="data">输入实体</param>
        /// <returns></returns>
        public static Result[] ConvertInto<T, Result> (this Result[] result, T[] data) where T : class
        {
            return _Mapper.Mapper(data, result);
        }
        /// <summary>
        /// 根据输入实体初始化输出实体
        /// </summary>
        /// <typeparam name="T">输入</typeparam>
        /// <typeparam name="Result">输出</typeparam>
        /// <param name="result">输出实体</param>
        /// <param name="data">输入实体</param>
        /// <param name="action">转换结果处理（参数: 输出，输入）</param>
        /// <returns></returns>
        public static Result ConvertInto<T, Result> (this Result result, T data, Func<Result, T, Result> action) where T : class
        {
            return action(_Mapper.Mapper(data, result), data);
        }
        public static Result[] ConvertMap<T, T1, Result> (this T[] array, T1[] datas, Func<T, T1, bool> match, Action<Result, T, T1> action) where T : class
        {
            List<Result> list = [];
            foreach (T i in array)
            {
                Result data = _Mapper.Mapper<T, Result>(i);
                foreach (T1 k in datas)
                {
                    if (match(i, k))
                    {
                        action(data, i, k);
                        break;
                    }
                }
                list.Add(data);
            }
            return list.ToArray();
        }

        public static Result[] ConvertMap<T, T1, Result> (this T[] array, T1[] datas, Func<T, T1, bool> match, Action<Result, T1> action) where T : class
        {
            List<Result> list = [];
            foreach (T i in array)
            {
                Result data = _Mapper.Mapper<T, Result>(i);
                foreach (T1 k in datas)
                {
                    if (match(i, k))
                    {
                        action(data, k);
                        break;
                    }
                }
                list.Add(data);
            }
            return list.ToArray();
        }
        public static Result[] ConvertMap<T, T1, Result> (this T[] array, T1[] datas, Func<T, bool> find, Func<T, T1, bool> match) where T : class where T1 : class
        {
            List<Result> list = [];
            foreach (T i in array)
            {
                if (find(i))
                {
                    Result data = _Mapper.Mapper<T, Result>(i);
                    foreach (T1 k in datas)
                    {
                        if (match(i, k))
                        {
                            data = _Mapper.Mapper<T1, Result>(k);
                            break;
                        }
                    }
                    list.Add(data);
                }
            }
            return list.ToArray();
        }
        public static Result[] ConvertMap<T, T1, Result> (this T[] array, T1[] datas, Func<T, T1, bool> match) where T : class where T1 : class
        {
            List<Result> list = [];
            foreach (T i in array)
            {
                Result data = _Mapper.Mapper<T, Result>(i);
                foreach (T1 k in datas)
                {
                    if (match(i, k))
                    {
                        data = _Mapper.Mapper<T1, Result>(k);
                        break;
                    }
                }
                list.Add(data);
            }
            return list.ToArray();
        }
    }
}
