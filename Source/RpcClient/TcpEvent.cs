using System;
using System.Net;

namespace RpcClient
{
        internal class TcpEvent : SocketTcpServer.Interface.ISocketEvent
        {
                public bool ClientBuildConnect(Guid clientId, IPEndPoint ipAddress, string[] arg, out string bindParam, out string error)
                {
                        if (!RpcClient.CheckAccreditIp(ipAddress.Address.ToString()))
                        {
                                bindParam = null;
                                error = "rpc.no.allow.con";
                                RpcLogSystem.AddConLog(ipAddress, error);
                                return false;
                        }
                        RpcLogSystem.AddConLog(clientId, ipAddress);
                        bindParam = null;
                        error = null;
                        return true;
                }

                public void ClientConnectClose(Guid clientId, string bindParam)
                {
                }
        }
}
