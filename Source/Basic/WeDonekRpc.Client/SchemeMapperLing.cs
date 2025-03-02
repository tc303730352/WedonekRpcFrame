using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;

namespace WeDonekRpc.Client
{
        public static class SchemeMapperLing
        {
                private static readonly IMapperCollect _Mapper = null;

                static SchemeMapperLing ()
                {
                        _Mapper = RpcClient.Ioc.Resolve<IMapperCollect> ();
                }

                /// <summary>
                /// 集合实体转换
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <typeparam name="Result"></typeparam>
                /// <param name="data"></param>
                /// <param name="scheme"></param>
                /// <returns></returns>
                public static Result[] ConvertMap<T, Result> (this IEnumerable<T> data, string scheme) where T : class
                {
                        IMapperHandler mapper = _Mapper.GetMapper (scheme);
                        return mapper.Mapper<T, Result> (data.ToArray ());
                }
                public static Result[] ConvertMap<T, Result> (this IEnumerable<T> data, Func<T, Result, Result> func, string scheme) where T : class
                {
                        IMapperHandler mapper = _Mapper.GetMapper (scheme);
                        return data.ToArray ().ConvertAll (a =>
                        {
                                Result to = mapper.Mapper<T, Result> (a);
                                return func (a, to);
                        });
                }
                public static To ConvertMap<From, To> (this From from, string scheme) where From : class
                {
                        IMapperHandler mapper = _Mapper.GetMapper (scheme);
                        return mapper.Mapper<From, To> (from);
                }
                public static To ConvertMap<From, To> (this From from, Func<From, To, To> func, string scheme) where From : class
                {
                        IMapperHandler mapper = _Mapper.GetMapper (scheme);
                        return func (from, mapper.Mapper<From, To> (from));
                }
                public static To[] ConvertMap<From, To> (this From[] from, string scheme) where From : class
                {
                        IMapperHandler mapper = _Mapper.GetMapper (scheme);
                        return mapper.Mapper<From[], To[]> (from);
                }
                public static To[] ConvertMap<From, To> (this From[] from, Func<From, To, To> func, string scheme) where From : class
                {
                        IMapperHandler mapper = _Mapper.GetMapper (scheme);
                        return from.ConvertAll (a =>
                        {
                                To to = mapper.Mapper<From, To> (a);
                                return func (a, to);
                        });
                }
                public static To[] ConvertMap<From, To> (this From[] from, Predicate<From> match, Func<From, To, To> func, string scheme) where From : class
                {
                        IMapperHandler mapper = _Mapper.GetMapper (scheme);
                        return from.Convert (match, a =>
                        {
                                To to = mapper.Mapper<From, To> (a);
                                return func (a, to);
                        });
                }
                public static To[] ConvertMap<From, To> (this From[] from, Predicate<From> match, string scheme) where From : class
                {
                        IMapperHandler mapper = _Mapper.GetMapper (scheme);
                        return mapper.Mapper<From[], To[]> (from.FindAll (match));
                }

                public static List<To> ConvertMap<From, To> (this List<From> from, string scheme) where From : class
                {
                        IMapperHandler mapper = _Mapper.GetMapper (scheme);
                        return mapper.Mapper<List<From>, List<To>> (from);
                }
                public static List<To> ConvertMap<From, To> (this List<From> from, Predicate<From> match, string scheme) where From : class
                {
                        IMapperHandler mapper = _Mapper.GetMapper (scheme);
                        return mapper.Mapper<List<From>, List<To>> (from.FindAll (match));
                }
                public static To ConvertFind<From, To> (this From[] from, Predicate<From> match, string scheme) where From : class
                {
                        IMapperHandler mapper = _Mapper.GetMapper (scheme);
                        return mapper.Mapper<From, To> (from.Find (match));
                }
                public static To ConvertFind<From, To> (this List<From> from, Predicate<From> match, string scheme) where From : class
                {
                        IMapperHandler mapper = _Mapper.GetMapper (scheme);
                        return mapper.Mapper<From, To> (from.Find (match));
                }


                /// <summary>
                /// 根据输入实体初始化输出实体
                /// </summary>
                /// <typeparam name="T">输入</typeparam>
                /// <typeparam name="Result">输出</typeparam>
                /// <param name="result">输出实体</param>
                /// <param name="data">输入实体</param>
                /// <param name="scheme">方案名称</param>
                /// <returns></returns>
                public static Result ConvertInto<T, Result> (this Result result, T data, string scheme) where T : class
                {
                        IMapperHandler mapper = _Mapper.GetMapper (scheme);
                        return mapper.Mapper (data, result);
                }
                /// <summary>
                /// 根据输入实体数组初始化输出实体数组
                /// </summary>
                /// <typeparam name="T">输入</typeparam>
                /// <typeparam name="Result">输出</typeparam>
                /// <param name="result">输出实体</param>
                /// <param name="data">输入实体</param>
                /// <returns></returns>
                public static Result[] ConvertInto<T, Result> (this Result[] result, T[] data, string scheme) where T : class
                {
                        IMapperHandler mapper = _Mapper.GetMapper (scheme);
                        return mapper.Mapper (data, result);
                }
                /// <summary>
                /// 根据输入实体初始化输出实体
                /// </summary>
                /// <typeparam name="T">输入</typeparam>
                /// <typeparam name="Result">输出</typeparam>
                /// <param name="result">输出实体</param>
                /// <param name="data">输入实体</param>
                /// <param name="action">转换结果处理（参数: 输出，输入）</param>
                /// <param name="scheme">方案名</param>
                /// <returns></returns>
                public static Result ConvertInto<T, Result> (this Result result, T data, Func<Result, T, Result> action, string scheme) where T : class
                {
                        IMapperHandler mapper = _Mapper.GetMapper (scheme);
                        return action (mapper.Mapper (data, result), data);
                }
                public static Result[] ConvertMap<T, T1, Result> (this T[] array, T1[] datas, Func<T, T1, bool> match, Action<Result, T, T1> action, string scheme) where T : class
                {
                        IMapperHandler mapper = _Mapper.GetMapper (scheme);
                        List<Result> list = new List<Result> ();
                        foreach (T i in array)
                        {
                                Result data = mapper.Mapper<T, Result> (i);
                                foreach (T1 k in datas)
                                {
                                        if (match (i, k))
                                        {
                                                action (data, i, k);
                                                break;
                                        }
                                }
                                list.Add (data);
                        }
                        return list.ToArray ();
                }

                public static Result[] ConvertMap<T, T1, Result> (this T[] array, T1[] datas, Func<T, T1, bool> match, Action<Result, T1> action, string scheme) where T : class
                {
                        IMapperHandler mapper = _Mapper.GetMapper (scheme);
                        List<Result> list = new List<Result> ();
                        foreach (T i in array)
                        {
                                Result data = mapper.Mapper<T, Result> (i);
                                foreach (T1 k in datas)
                                {
                                        if (match (i, k))
                                        {
                                                action (data, k);
                                                break;
                                        }
                                }
                                list.Add (data);
                        }
                        return list.ToArray ();
                }
                public static Result[] ConvertMap<T, T1, Result> (this T[] array, T1[] datas, Func<T, bool> find, Func<T, T1, bool> match, string scheme) where T : class where T1 : class
                {
                        IMapperHandler mapper = _Mapper.GetMapper (scheme);
                        List<Result> list = new List<Result> ();
                        foreach (T i in array)
                        {
                                if (find (i))
                                {
                                        Result data = mapper.Mapper<T, Result> (i);
                                        foreach (T1 k in datas)
                                        {
                                                if (match (i, k))
                                                {
                                                        data = mapper.Mapper<T1, Result> (k);
                                                        break;
                                                }
                                        }
                                        list.Add (data);
                                }
                        }
                        return list.ToArray ();
                }
                public static Result[] ConvertMap<T, T1, Result> (this T[] array, T1[] datas, Func<T, T1, bool> match, string scheme) where T : class where T1 : class
                {
                        IMapperHandler mapper = _Mapper.GetMapper (scheme);
                        List<Result> list = new List<Result> ();
                        foreach (T i in array)
                        {
                                Result data = mapper.Mapper<T, Result> (i);
                                foreach (T1 k in datas)
                                {
                                        if (match (i, k))
                                        {
                                                data = mapper.Mapper<T1, Result> (k);
                                                break;
                                        }
                                }
                                list.Add (data);
                        }
                        return list.ToArray ();
                }
        }
}
