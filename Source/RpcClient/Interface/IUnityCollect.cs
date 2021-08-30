using System;
using System.Reflection;

using RpcClient.Model;

namespace RpcClient.Interface
{
        public delegate void Register(IocBody body);
        public delegate bool Registering(IocBody body);
        /// <summary>
        /// IOC容器
        /// </summary>
        public interface IUnityCollect
        {
                /// <summary>
                /// 注册事件
                /// </summary>
                event Register RegisterEvent;
                /// <summary>
                /// 注册前
                /// </summary>
                event Registering Registering;
                /// <summary>
                /// 自动加载
                /// </summary>
                /// <param name="assemblyName">应用程序域</param>
                void Load(string assemblyName);
                /// <summary>
                /// 自动加载
                /// </summary>
                /// <param name="assembly">应用程序域</param>
                void Load(Assembly assembly);
                /// <summary>
                /// 自动加载
                /// </summary>
                /// <param name="assembly">应用程序域</param>
                /// <param name="Interfaces">接口所在程序域</param>
                void Load(Assembly assembly, Assembly Interfaces);
                /// <summary>
                /// 自动加载
                /// </summary>
                /// <param name="assembly">应用程序域</param>
                /// <param name="type">需继承的接口</param>
                void Load(Assembly assembly, Type type);
                /// <summary>
                /// 单独注册
                /// </summary>
                /// <param name="form"></param>
                /// <param name="to"></param>
                /// <param name="name"></param>
                /// <returns></returns>
                bool Register(Type form, Type to, string name);
                /// <summary>
                /// 单独注册
                /// </summary>
                /// <param name="form"></param>
                /// <param name="to"></param>
                /// <returns></returns>
                bool Register(Type form, Type to);
                /// <summary>
                /// 注册类型（递归扫描该类型下的所有构造函数类型自动注册）
                /// </summary>
                /// <param name="type">注册类型</param>
                void Register(Type type);
                /// <summary>
                /// 注册固定类型实例
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="data"></param>
                /// <returns></returns>
                bool RegisterType<T>(Type to);

                /// <summary>
                /// 注册固定类型实例
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="data"></param>
                /// <returns></returns>
                bool RegisterType<T>(Type to, string name);
                /// <summary>
                /// 单独注册
                /// </summary>
                /// <param name="form"></param>
                /// <param name="to"></param>
                /// <returns></returns>
                bool RegisterType(Type form, Type to);
                /// <summary>
                /// 注册
                /// </summary>
                /// <param name="form"></param>
                /// <param name="to"></param>
                /// <param name="name">自定义名称</param>
                /// <returns></returns>
                bool RegisterType(Type form, Type to, string name);
                /// <summary>
                /// 获取对象
                /// </summary>
                /// <param name="form"></param>
                /// <returns></returns>
                object Resolve(Type form);
                /// <summary>
                /// 获取对象
                /// </summary>
                /// <param name="form"></param>
                /// <param name="name"></param>
                /// <returns></returns>
                object Resolve(Type form, string name);
                /// <summary>
                /// 获取对象
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <returns></returns>
                T Resolve<T>();
                /// <summary>
                /// 获取对象
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="name"></param>
                /// <returns></returns>
                T Resolve<T>(string name);
                /// <summary>
                /// 注册单例
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="data"></param>
                /// <returns></returns>
                bool RegisterInstance<T>(T data);
                bool RegisterInstance<T>(T data, string name);

        }
}