using System;
using System.Net;
using WeDonekRpc.Client.Log;
using WeDonekRpc.TcpServer.Interface;

namespace WeDonekRpc.Client
{
    internal class TcpEvent : ISocketEvent
    {
        public bool ClientBuildConnect (Guid clientId, IPEndPoint ipAddress, string[] arg, out string bindParam, out string error)
        {
            if (!RpcClient.CheckAccreditIp(ipAddress.Address.ToString()))
            {
                bindParam = null;
                error = "rpc.no.allow.con";
                RpcLogSystem.AddConFailLog(ipAddress, arg);
                return false;
            }
            RpcLogSystem.AddConLog(clientId, ipAddress);
            bindParam = arg[0];
            error = null;
            return true;
        }

        public void ClientConnectClose (Guid clientId, string bindParam)
        {
        }
    }
}
