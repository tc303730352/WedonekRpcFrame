using RpcCentral.Collect.Controller;
using RpcCentral.Common;
using RpcCentral.Common.Config;
using RpcCentral.Model;
using WeDonekRpc.Helper.Email;

namespace RpcCentral.Collect
{
    public static class LinqHelper
    {
        public static int GetVer (this string ver)
        {
            string[] t = ver.Split('.');
            if (t.Length == 1)
            {
                return int.Parse(ver) * 10000;
            }
            else if (t.Length == 2)
            {
                return ( int.Parse(t[0]) * 10000 ) + ( int.Parse(t[1]) * 100 );
            }
            return ( int.Parse(t[0]) * 10000 ) + ( int.Parse(t[1]) * 100 ) + int.Parse(t[2]);
        }
        public static void SendOnline (this RpcServerController server)
        {
            if (!RpcContralConfig.SysConfig.ServerStateNotice || RpcContralConfig.SysConfig.EmailList.Length == 0)
            {
                return;
            }
            RemoteServerModel datum = server.Server;
            RpcServerSysConfig sysConfig = RpcContralConfig.SysConfig;
            string content = string.Format("服务器( {0} ) 恢复!</br>服务器IP:{1}:{2}</br>服务器ID：{3} </br>服务器Type: {4}", datum.ServerName, datum.RemoteIp, datum.ServerPort, server.ServerId, server.GroupTypeVal);
            EmailModel model = new EmailModel
            {
                DisplayName = sysConfig.DisplayName,
                Reciver = sysConfig.EmailList,
                Content = content,
                Title = string.Format("RPC服务端恢复-{0}", datum.ServerName),
                EmailAccount = sysConfig.EmailAccount,
                EmailPwd = sysConfig.EmailPwd
            };
            EmailTools.SendEmail(model);
        }
        public static void SendOffline (this RpcServerController server)
        {
            if (!RpcContralConfig.SysConfig.ServerStateNotice || RpcContralConfig.SysConfig.EmailList.Length == 0)
            {
                return;
            }
            RemoteServerModel datum = server.Server;
            RpcServerSysConfig sysConfig = RpcContralConfig.SysConfig;
            string content = string.Format("服务器( {0} ) 离线!</br>服务器IP:{1}:{2}</br>服务器ID：{3} </br>服务器Type:{4}", datum.ServerName, datum.RemoteIp, datum.ServerPort, server.ServerId, server.GroupTypeVal);
            EmailModel model = new EmailModel
            {
                DisplayName = sysConfig.DisplayName,
                Reciver = sysConfig.EmailList,
                Content = content,
                Title = string.Format("RPC服务端离线-{0}", datum.ServerName),
                EmailAccount = sysConfig.EmailAccount,
                EmailPwd = sysConfig.EmailPwd
            };
            EmailTools.SendEmail(model);
        }
    }
}
