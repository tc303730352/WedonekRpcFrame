using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

using RpcClient.Config;
using RpcClient.Helper;
using RpcClient.Interface;
using RpcClient.Model;
using RpcClient.RouteDelegate;

using RpcModel;

namespace RpcClient.Collect
{
        [Attr.ClassLifetimeAttr(Attr.ClassLifetimeType.单例)]
        internal class TcpRouteCollect : ITcpRouteCollect
        {
                static TcpRouteCollect()
                {
                        RpcMsgCollect.SetMsgEvent(_RemoteMsgEvent);
                        RpcQueueCollect.SetMsgEvent(_QueueMsgEvent);
                }



                private static readonly ConcurrentDictionary<string, IRoute> _RouteList = new ConcurrentDictionary<string, IRoute>();

                public event Action<IRoute> RegRouteEvent;

                public void AddRoute(IRoute route)
                {
                        if (_RouteList.TryAdd(route.RouteName, route) && RegRouteEvent != null)
                        {
                                RegRouteEvent(route);
                        }
                }

                public void RemoveRoute(string name)
                {
                        if (_RouteList.ContainsKey(name))
                        {
                                _RouteList.TryRemove(name, out _);
                        }
                }
                public bool CheckIsExists(string route)
                {
                        return _RouteList.ContainsKey(route);
                }

                public void Dispose()
                {
                        _RouteList.Clear();
                }
                #region 添加路由

                public void AddRoute<T, Result>(string name, RpcActionPaging<T, Result> action)
                {
                        this._AddRoute(name, action);
                }
                public void AddRoute<T, Result>(string name, RpcFuncPaging<T, Result> action)
                {
                        this._AddRoute(name, action);
                }
                public void AddRoute<T, Result>(string name, RpcFuncPagingSource<T, Result> action)
                {
                        this._AddRoute(name, action);
                }
                public void AddRoute<T, Result>(string name, RpcOutPagingSource<T, Result> action)
                {
                        this._AddRoute(name, action);
                }
                public void AddRoute<T, Result>(string name, RpcOutPaging<T, Result> action)
                {
                        this._AddRoute(name, action);
                }
                public void AddRoute<T, Result>(string name, RpcActionPagingSource<T, Result> action)
                {
                        this._AddRoute(name, action);
                }
                public void AddRoute<T>(string name, RpcFunc<T> action)
                {
                        this._AddRoute(name, action);
                }
                public void AddRoute<T, Result>(string name, RpcFunc<T, Result> action)
                {
                        this._AddRoute(name, action);
                }
                public void AddRoute<T, Result>(string name, RpcFuncSource<T, Result> action)
                {
                        this._AddRoute(name, action);
                }
                public void AddRoute<T>(string name, RpcActionSource<T> action)
                {
                        this._AddRoute(name, action);
                }
                public void AddRoute<Result>(string name, RpcActionReturn<Result> action)
                {
                        this._AddRoute(name, action);
                }
                public void AddRoute<Result>(string name, RpcOutReturn<Result> action)
                {
                        this._AddRoute(name, action);
                }
                public void AddRoute<T>(string name, RpcOut<T> action)
                {
                        this._AddRoute(name, action);
                }
                public void AddRoute(string name, RpcOut action)
                {
                        this._AddRoute(name, action);
                }
                public void AddRoute<T, Result>(string name, RpcOut<T, Result> action)
                {
                        this._AddRoute(name, action);
                }

                public void AddRoute<T>(string name, RpcOutSource<T> action)
                {
                        this._AddRoute(name, action);
                }
                public void AddRoute<T, Result>(string name, RpcOutSource<T, Result> action)
                {
                        this._AddRoute(name, action);
                }
                public void AddRoute(string name, RpcAction action)
                {
                        this._AddRoute(name, action);
                }
                public void AddRoute<T>(string name, RpcAction<T> action)
                {
                        this._AddRoute(name, action);
                }
                public void AddRoute<T, Result>(string name, RpcAction<T, Result> action)
                {
                        this._AddRoute(name, action);
                }
                public void AddRoute<T, Result>(string name, RpcActionSource<T, Result> action)
                {
                        this._AddRoute(name, action);
                }
                public void AddRoute<T>(RpcAction<T> action)
                {
                        this._AddRoute(typeof(T).Name, action);
                }

                public void AddRoute<T>(RpcFunc<T> action)
                {
                        this._AddRoute(typeof(T).Name, action);
                }

                public void AddRoute<T>(RpcOut<T> action)
                {
                        this._AddRoute(typeof(T).Name, action);
                }

                public void AddRoute<T>(RpcOutSource<T> action)
                {
                        this._AddRoute(typeof(T).Name, action);
                }
                #endregion


                #region 私有方法
                private ITcpRoute _CreateDelegate(string name, Delegate source)
                {
                        MethodInfo method = source.Method;
                        if (method.ReturnType == ConfigDic.VoidType)
                        {
                                return new VoidFuncDelegate(name, source);
                        }
                        else if (method.ReturnType == ConfigDic.ReturnType)
                        {
                                return new RouteDelegate.RouteDelegate(name, source);
                        }
                        else if (RpcClientHelper.CheckIsOutRoute(method))
                        {
                                return new OutFuncDelegate(name, source);
                        }
                        else
                        {
                                return new ReturnFuncDelegate(name, source);
                        }
                }
                private void _AddRoute(string name, Delegate source)
                {
                        if (_RouteList.ContainsKey(name))
                        {
                                return;
                        }
                        ITcpRoute route = this._CreateDelegate(name, source);
                        this.AddRoute(route);
                }
                private static IBasicRes _MsgEvent(IRoute route, IMsg msg)
                {
                        return route.TcpMsgEvent(msg);
                }
                private static IBasicRes _QueueMsgEvent(IMsg msg)
                {
                        if (_RouteList.TryGetValue(msg.MsgKey, out IRoute route))
                        {
                                return _MsgEvent(route, msg);
                        }
                        return null;
                }
                private static IBasicRes _RemoteMsgEvent(IMsg msg)
                {
                        if (_RouteList.TryGetValue(msg.MsgKey, out IRoute route))
                        {
                                return _MsgEvent(route, msg);
                        }
                        return new BasicRes("rpc.route.no.find");
                }

                public IRoute[] GetRoutes()
                {
                        return _RouteList.Values.ToArray();
                }


                #endregion

        }
}