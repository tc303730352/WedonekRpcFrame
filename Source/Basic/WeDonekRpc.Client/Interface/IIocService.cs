using System;
using WeDonekRpc.Client.Ioc;

namespace WeDonekRpc.Client.Interface
{

    /// <summary>
    /// IOC容器
    /// </summary>
    public interface IIocService
    {
        /// <summary>
        /// 创建临时生命周期对象(不在当前线程传播)
        /// </summary>
        /// <returns></returns>
        IocScope CreateTempScore ();
        /// <summary>
        /// 获取范围生命周期
        /// </summary>
        /// <returns></returns>
        IocScope CreateScore ();
        /// <summary>
        /// 获取范围生命周期
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IocScope CreateScore (object key);
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        object Resolve (Type form);
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="form"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        object Resolve (Type form, string name);
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Resolve<T> ();
        /// <summary>
        /// 尝试获取对象无责注册
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        bool TryResolve<T> (out T data) where T : class;
        bool TryResolve<T> (string name, out T data) where T : class;
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        T Resolve<T> (string name);

        bool IsRegistered (Type type);
        bool IsRegistered (Type type, string name);
    }
}