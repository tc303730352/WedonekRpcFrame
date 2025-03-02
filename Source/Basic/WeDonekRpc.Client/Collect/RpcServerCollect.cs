using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using WeDonekRpc.Client.Server;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Validate;

namespace WeDonekRpc.Client.Collect
{
    internal class RpcServerCollect
    {
        private static readonly Timer _CheckIsRestoreTimer;
        private static readonly RpcServer[] _RpcServerList = null;
        private static readonly RpcServer _CurrentServer = null;

        static RpcServerCollect ()
        {
            _RpcServerList = _GetRpcServer();
            if (_RpcServerList.Length > 0)
            {
                _CurrentServer = _RpcServerList[0];
            }
            _CheckIsRestoreTimer = new Timer(_CheckIsRestore, null, 1000, 1000);
        }

        public static bool GetServer (out RpcServer server)
        {
            if (_CurrentServer == null)
            {
                server = null;
                return false;
            }
            else if (_RpcServerList.Length == 1)
            {
                server = _CurrentServer;
                return server.IsUsable;
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
        private static void _CheckIsRestore (object state)
        {
            if (_RpcServerList.IsNull())
            {
                return;
            }
            _RpcServerList.ForEach(a =>
            {
                a.CheckIsRestore();
            });
        }
        public static bool GetServer (out RpcServer server, string rid)
        {
            if (_RpcServerList.Length > 1)
            {
                server = _RpcServerList.Find(a => a.ServerId != rid && a.IsUsable);
                return server != null;
            }
            else if (_CurrentServer.CheckIsUsable())
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

        private static RpcServer[] _GetRpcServer ()
        {
            string[] address = Config.WebConfig.BasicConfig.RpcServer;
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
                else if (a.Validate(ValidateFormat.HOST))
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
