using System;
using WeDonekRpc.Client.RouteDelegate;

namespace WeDonekRpc.Client.Interface
{
    /// <summary>
    /// RPC路由管理集合
    /// </summary>
    public interface IRouteService : IDisposable
    {
        /// <summary>
        /// 获取所有路由
        /// </summary>
        /// <returns></returns>
        IRoute[] GetRoutes ();
        /// <summary>
        /// 路由注册事件
        /// </summary>
        event Action<IRoute> RegRouteEvent;
        /// <summary>
        /// 检查路由是否存在
        /// </summary>
        /// <param name="route">路由地址</param>
        /// <returns></returns>
        bool CheckIsExists (string route);
        /// <summary>
        /// 注册路由
        /// </summary>
        /// <param name="name">路由地址</param>
        /// <param name="action">回调的委托</param>
        void AddRoute (string name, RpcAction action);

        /// <summary>
        /// 注册路由
        /// </summary>
        /// <param name="route">路由对象</param>
        void AddRoute (IRoute route);
        /// <summary>
        /// 注册路由
        /// </summary>
        /// <typeparam name="T">参数对象</typeparam>
        /// <param name="name">路由地址</param>
        /// <param name="action">回调委托</param>
        void AddRoute<T> (string name, RpcActionSource<T> action);
        /// <summary>
        /// 注册路由
        /// </summary>
        ///<typeparam name="T">参数对象</typeparam>
        /// <typeparam name="Result">执行结果</typeparam>
        ///  <param name="name">路由地址</param>
        /// <param name="action">回调委托</param>
        void AddRoute<T, Result> (string name, RpcFunc<T, Result> action);

        /// <summary>
        /// 注册路由
        /// </summary>
        ///<typeparam name="T">参数对象</typeparam>
        /// <typeparam name="Result">执行结果</typeparam>
        ///  <param name="name">路由地址</param>
        /// <param name="action">回调委托</param>
        void AddRoute<T, Result> (string name, RpcFuncSource<T, Result> action);


        /// <summary>
        /// 注册路由
        /// </summary>
        ///<typeparam name="T">参数对象</typeparam>
        ///  <param name="name">路由地址</param>
        /// <param name="action">回调委托</param>
        void AddRoute<T> (string name, RpcAction<T> action);
        /// <summary>
        /// 注册路由
        /// </summary>
        ///<typeparam name="T">参数对象</typeparam>
        ///  <param name="name">路由地址</param>
        /// <param name="action">回调委托</param>
        void AddRoute<T> (string name, RpcFunc<T> action);


        /// <summary>
        /// 注册路由
        /// </summary>
        ///<typeparam name="T">参数对象</typeparam>
        /// <param name="action">回调委托</param>
        void AddRoute<T> (RpcAction<T> action);
        /// <summary>
        /// 注册路由
        /// </summary>
        ///<typeparam name="T">参数对象</typeparam>
        /// <param name="action">回调委托</param>
        void AddRoute<T> (RpcFunc<T> action);


        /// <summary>
        /// 注册路由
        /// </summary>
        /// <param name="name">路由地址</param>
        /// <param name="action">回调的委托</param>
        void AddRoute (string name, RpcTaskAction action);

        /// <summary>
        /// 注册路由
        /// </summary>
        /// <typeparam name="T">参数对象</typeparam>
        /// <param name="name">路由地址</param>
        /// <param name="action">回调委托</param>
        void AddRoute<T> (string name, RpcTaskActionSource<T> action);
        /// <summary>
        /// 注册路由
        /// </summary>
        ///<typeparam name="T">参数对象</typeparam>
        /// <typeparam name="Result">执行结果</typeparam>
        ///  <param name="name">路由地址</param>
        /// <param name="action">回调委托</param>
        void AddRoute<T, Result> (string name, RpcTaskFunc<T, Result> action);

        /// <summary>
        /// 注册路由
        /// </summary>
        ///<typeparam name="T">参数对象</typeparam>
        /// <typeparam name="Result">执行结果</typeparam>
        ///  <param name="name">路由地址</param>
        /// <param name="action">回调委托</param>
        void AddRoute<T, Result> (string name, RpcTaskFuncSource<T, Result> action);


        /// <summary>
        /// 注册路由
        /// </summary>
        ///<typeparam name="T">参数对象</typeparam>
        ///  <param name="name">路由地址</param>
        /// <param name="action">回调委托</param>
        void AddRoute<T> (string name, RpcTaskAction<T> action);
        /// <summary>
        /// 注册路由
        /// </summary>
        ///<typeparam name="T">参数对象</typeparam>
        ///  <param name="name">路由地址</param>
        /// <param name="action">回调委托</param>
        void AddRoute<T> (string name, RpcTaskFunc<T> action);


        /// <summary>
        /// 注册路由
        /// </summary>
        ///<typeparam name="T">参数对象</typeparam>
        /// <param name="action">回调委托</param>
        void AddRoute<T> (RpcTaskAction<T> action);
        /// <summary>
        /// 注册路由
        /// </summary>
        ///<typeparam name="T">参数对象</typeparam>
        /// <param name="action">回调委托</param>
        void AddRoute<T> (RpcTaskFunc<T> action);
        /// <summary>
        /// 移除路由
        /// </summary>
        /// <param name="name">路由地址</param>
        void RemoveRoute (string name);
    }
}