using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using RpcService.Model;

using RpcHelper;

namespace RpcService.Collect
{
        internal class TcpRouteCollect : SocketTcpServer.Interface.IAllot
        {
                private static readonly Dictionary<string, ITcpRoute> _RouteList = new Dictionary<string, ITcpRoute>();
                public static void AddRoute(ITcpRoute route)
                {
                        if (_RouteList.ContainsKey(route.RouteName))
                        {
                                return;
                        }
                        _RouteList.Add(route.RouteName, route);
                }
                public override object Action()
                {
                        RemoteMsg msg = new RemoteMsg(this.GetData(), this.ClientIp.Address.ToString());
                        if (_RouteList.TryGetValue(this.Type, out ITcpRoute route))
                        {
                                return route.TcpMsgEvent(msg);
                        }
                        return null;
                }

                public static void InitRoute()
                {
                        SocketTcpServer.Config.SocketConfig.DefaultAllot = new TcpRouteCollect();
                        SocketTcpServer.Config.SocketConfig.DefaultServerPort = 983;
                        SocketTcpServer.Config.SocketConfig.SocketEvent = new TcpSocketEvent();
                        _LoadRoute("RpcService");
                        SocketTcpServer.SocketTcpServer.Init();
                }
                private static void _LoadRoute(string assemblyName)
                {
                        Assembly[] assembly = AppDomain.CurrentDomain.GetAssemblies();
                        Assembly obj = assembly.FirstOrDefault(a => a.GetName().Name == assemblyName);
                        if (obj != null)
                        {
                                Type iType = typeof(ITcpRoute);
                                ITcpRoute route = null;
                                Array.ForEach(obj.GetTypes(), a =>
                                {
                                        Type type = a.GetInterface(iType.Name);
                                        if (type != null && a.IsClass && a.GetConstructors().Count(b =>
                                        {
                                                ParameterInfo[] param = b.GetParameters();
                                                if (param == null || param.Length == 0)
                                                {
                                                        return true;
                                                }
                                                return false;
                                        }) != 0)
                                        {
                                                route = (ITcpRoute)obj.CreateInstance(a.FullName, true);
                                                if (!string.IsNullOrEmpty(route.RouteName))
                                                {
                                                        AddRoute(route);
                                                }
                                        }
                                });
                        }
                }

        }
}
