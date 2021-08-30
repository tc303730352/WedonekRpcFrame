using System;
using System.Net;

using SocketTcpServer.Interface;

using RpcHelper;
namespace RpcService
{
        internal class TcpSocketEvent : ISocketEvent
        {
                public bool ClientBuildConnect(Guid clientId, IPEndPoint ipAddress, string[] arg, out string bindParam, out string error)
                {
                        if (arg == null || arg.Length == 0)
                        {
                                bindParam = null;
                                error = "public.param.null";
                                return false;
                        }
                        if (Collect.RpcMerCollect.CheckConAccredit(ipAddress.Address.ToString(), arg[0], out error))
                        {
                                bindParam = arg[0];
                                return true;
                        }
                        bindParam = null;
                        new WarnLog(error, "服务链接,已被拒绝!")
                        {
                                { "Ip", ipAddress },
                                { "AppId", arg[0] }
                        }.Save();
                        return false;
                }



                public void ClientConnectClose(Guid clientId, string bindParam)
                {
                }
        }
}
