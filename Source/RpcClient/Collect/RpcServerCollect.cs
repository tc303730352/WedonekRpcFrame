using System.Collections.Generic;
using System.Linq;
using System.Net;

using RpcClient.Server;

using RpcHelper;
using RpcHelper.Validate;

namespace RpcClient.Collect
{
        internal class RpcServerCollect
        {
                private static readonly RpcServer[] _RpcServerList = null;
                private static readonly RpcServer _CurrentServer = null;

                static RpcServerCollect()
                {
                        _RpcServerList = _GetRpcServer();
                        if (_RpcServerList.Length > 0)
                        {
                                _CurrentServer = _RpcServerList[0];
                        }
                }

                public static bool GetServer(out RpcServer server)
                {
                        if (_RpcServerList.Length == 1)
                        {
                                server = _CurrentServer;
                                return true;
                        }
                        else if (_CurrentServer.IsUsable)
                        {
                                server = _CurrentServer;
                                return true;
                        }
                        else
                        {
                                server = _RpcServerList.Find(a => a.IsUsable);
                        }
                        return server != null;
                }
                public static bool GetServer(out RpcServer server, string rid)
                {
                        if (_RpcServerList.Length > 1)
                        {
                                server = _RpcServerList.Find(a => a.ServerId != rid && a.IsUsable);
                                return server != null;
                        }
                        else if (_CurrentServer.IsUsable && _CurrentServer.CheckIsUsable())
                        {
                                server = _CurrentServer;
                                return true;
                        }
                        else
                        {
                                server = null;
                                return false;
                        }
                }
                private static RpcServer[] _GetRpcServer()
                {
                        string[] address = Config.WebConfig.RpcServer;
                        List<RpcServer> list = new List<RpcServer>(address.Length);
                        address.ForEach(a =>
                        {
                                if (a.IsNull())
                                {
                                        return;
                                }
                                else if (a.Validate(ValidateFormat.IP))
                                {
                                        list.Add(new RpcServer(a));
                                }
                                else if (a.Validate(ValidateFormat.URL))
                                {
                                        int port = 0;
                                        if (a.IndexOf(":") != -1)
                                        {
                                                string[] t = a.Split(':');
                                                port = int.Parse(t[1]);
                                                a = t[0].Trim();
                                        }
                                        IPAddress[] ipList = Dns.GetHostAddresses(a);
                                        if (!ipList.IsNull())
                                        {
                                                ipList.Distinct(b => b.ToString()).ForEach(b =>
                                                  {
                                                          list.Add(new RpcServer(b, port));
                                                  });
                                        }
                                }
                        });
                        return list.ToArray();
                }

        }
}
