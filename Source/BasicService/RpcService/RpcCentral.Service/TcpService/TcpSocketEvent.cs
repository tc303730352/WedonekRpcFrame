using System.Net;
using RpcCentral.Collect;
using RpcCentral.Common;
using WeDonekRpc.Helper.Log;
using WeDonekRpc.TcpServer.Interface;

namespace RpcCentral.Service.TcpService
{
    internal class TcpSocketEvent : ISocketEvent
    {
        private IRpcMerCollect _RpcMer => UnityHelper.Resolve<IRpcMerCollect>();
        public bool ClientBuildConnect ( Guid clientId, IPEndPoint ipAddress, string[] arg, out string bindParam, out string error )
        {
            if ( arg == null || arg.Length == 0 )
            {
                bindParam = null;
                error = "public.param.null";
                return false;
            }
            using ( IocScope socre = UnityHelper.CreateScore() )
            {
                if ( this._RpcMer.CheckConAccredit(ipAddress.Address.ToString(), arg[0], out error) )
                {
                    bindParam = arg[1];
                    return true;
                }
            }
            bindParam = arg[1];
            new WarnLog(error, "服务链接,已被拒绝!")
                        {
                                { "Ip", ipAddress },
                                { "AppId", arg[0] },
                                { "IdentityId", arg[1] }
                        }.Save();
            return false;
        }



        public void ClientConnectClose ( Guid clientId, string bindParam )
        {
        }
    }
}
