using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Config;
using WeDonekRpc.Client.Helper;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Log;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Client.RouteDelegate;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.RouteService
{
    [Attr.ClassLifetimeAttr(Attr.ClassLifetimeType.SingleInstance)]
    internal class RouteService : IRouteService
    {
        static RouteService ()
        {
            RpcMsgCollect.SetMsgEvent(_RemoteMsgEvent);
            RpcQueueCollect.SetMsgEvent(_QueueMsgEvent);
        }

        private static readonly ConcurrentDictionary<string, IRoute> _RouteList = new ConcurrentDictionary<string, IRoute>();

        public event Action<IRoute> RegRouteEvent;

        public void AddRoute (IRoute route)
        {
            if (_RouteList.TryAdd(route.RouteName, route) && RegRouteEvent != null)
            {
                RegRouteEvent(route);
            }
        }

        public void RemoveRoute (string name)
        {
            if (_RouteList.ContainsKey(name))
            {
                _ = _RouteList.TryRemove(name, out _);
            }
        }
        public bool CheckIsExists (string route)
        {
            return _RouteList.ContainsKey(route);
        }

        public void Dispose ()
        {
            _RouteList.Clear();
        }
        #region 添加路由


        public void AddRoute<T> (string name, RpcFunc<T> action)
        {
            this._AddRoute<T>(name, action);
        }
        public void AddRoute<T, Result> (string name, RpcFunc<T, Result> action)
        {
            this._AddRoute<T>(name, action);
        }
        public void AddRoute<T, Result> (string name, RpcFuncSource<T, Result> action)
        {
            this._AddRoute<T>(name, action);
        }
        public void AddRoute<T> (string name, RpcActionSource<T> action)
        {
            this._AddRoute<T>(name, action);
        }

        public void AddRoute (string name, RpcAction action)
        {
            this._AddRoute(name, action);
        }
        public void AddRoute<T> (string name, RpcAction<T> action)
        {
            this._AddRoute<T>(name, action);
        }

        public void AddRoute<T> (RpcAction<T> action)
        {
            this._AddRoute<T>(typeof(T).Name, action);
        }

        public void AddRoute<T> (RpcFunc<T> action)
        {
            this._AddRoute<T>(typeof(T).Name, action);
        }


        #endregion


        #region 私有方法
        private ITcpRoute _CreateDelegate (string name, Delegate source, string show)
        {
            MethodInfo method = source.Method;
            if (!method.IsStatic)
            {
                throw new ErrorException("rpc.route.delegate.can.only.static");
            }
            if (method.ReturnType == ConfigDic.TaskType)
            {
                return new VoidTaskFuncDelegate(name, source, show);
            }
            else if (method.ReturnType == ConfigDic.TaskReturnType)
            {
                return new BasicTaskRouteDelegate(name, source, show);
            }
            if (method.ReturnType == ConfigDic.VoidType)
            {
                return new VoidFuncDelegate(name, source, show);
            }
            else if (method.ReturnType == ConfigDic.ReturnType)
            {
                return new BasicRouteDelegate(name, source, show);
            }
            else if (method.ReturnType.Name == ConfigDic.TaskFuncType)
            {
                Type type = method.ReturnType.GetGenericArguments()[0];
                if (type.GetInterface(ConfigDic.ReturnType.FullName) != null)
                {
                    return new BasicTaskRouteDelegate(name, source, show);
                }
                return new ReturnTaskFuncDelegate(name, source, show);
            }
            else
            {
                return new ReturnFuncDelegate(name, source, show);
            }
        }
        private void _AddRoute (string name, Delegate source)
        {
            if (_RouteList.ContainsKey(name))
            {
                return;
            }
            ITcpRoute route = this._CreateDelegate(name, source, null);
            this.AddRoute(route);
        }
        private void _AddRoute<T> (string name, Delegate source)
        {
            if (_RouteList.ContainsKey(name))
            {
                return;
            }
            string show = XmlShowHelper.FindShow(typeof(T));
            ITcpRoute route = this._CreateDelegate(name, source, show);
            this.AddRoute(route);
        }
        private static IBasicRes _MsgEvent (IRoute route, IMsg msg)
        {
            return route.TcpMsgEvent(msg);
        }
        private static bool _QueueMsgEvent (IMsg msg)
        {
            if (_RouteList.TryGetValue(msg.MsgKey, out IRoute route))
            {
                IBasicRes res = _MsgEvent(route, msg);
                if (res.IsError)
                {
                    RpcLogSystem.AddQueueError(res.ErrorMsg, msg);
                    return false;
                }
                return true;
            }
            return false;
        }
        private static IBasicRes _RemoteMsgEvent (IMsg msg)
        {
            if (_RouteList.TryGetValue(msg.MsgKey, out IRoute route))
            {
                return _MsgEvent(route, msg);
            }
            return new BasicRes("rpc.route.no.find");
        }

        public IRoute[] GetRoutes ()
        {
            return _RouteList.Values.ToArray();
        }

        public void AddRoute (string name, RpcTaskAction action)
        {
            this._AddRoute(name, action);
        }

        public void AddRoute<T> (string name, RpcTaskActionSource<T> action)
        {
            this._AddRoute<T>(name, action);
        }

        public void AddRoute<T, Result> (string name, RpcTaskFunc<T, Result> action)
        {
            this._AddRoute<T>(name, action);
        }

        public void AddRoute<T, Result> (string name, RpcTaskFuncSource<T, Result> action)
        {
            this._AddRoute<T>(name, action);
        }

        public void AddRoute<T> (string name, RpcTaskAction<T> action)
        {
            this._AddRoute<T>(name, action);
        }

        public void AddRoute<T> (string name, RpcTaskFunc<T> action)
        {
            this._AddRoute<T>(name, action);
        }

        public void AddRoute<T> (RpcTaskAction<T> action)
        {
            this._AddRoute<T>(typeof(T).Name, action);
        }

        public void AddRoute<T> (RpcTaskFunc<T> action)
        {
            this._AddRoute<T>(typeof(T).Name, action);
        }


        #endregion

    }
}