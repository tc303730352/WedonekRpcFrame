using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Log;
using WeDonekRpc.TcpClient.Interface;
using WeDonekRpc.TcpClient.Model;

namespace WeDonekRpc.TcpClient.Manage
{
    internal class ClientManage
    {
        private static readonly ConcurrentDictionary<string, ServerInfo> _ServerList = new ConcurrentDictionary<string, ServerInfo>();


        private static readonly Timer _Heartbeat = new Timer(new TimerCallback(_SendHeartbeat), null, Config.SocketConfig.HeartbeatTime, Config.SocketConfig.HeartbeatTime);

        private static void _SendHeartbeat (object res)
        {
            string[] keys = _ServerList.Keys.ToArray();
            Array.ForEach(keys, a =>
            {
                if (_ServerList.TryGetValue(a, out ServerInfo server))
                {
                    server.SendHeartbeat();
                }
            });
        }
        public static void CloseClient (string serverId)
        {
            if (_ServerList.TryRemove(serverId, out ServerInfo server))
            {
                server.CloseClient();
            }
        }
        public static void CloseClient ()
        {
            string[] keys = _ServerList.Keys.ToArray();
            keys.ForEach(a =>
            {
                if (_ServerList.TryRemove(a, out ServerInfo server))
                {
                    server.CloseClient();
                }
            });
        }
        public static string GetServerId (string ip, int port)
        {
            string id = string.Format("{0}:{1}", ip, port);
            if (_ServerList.TryGetValue(id, out ServerInfo server))
            {
                return server.Id;
            }
            server = _ServerList.GetOrAdd(id, new ServerInfo(ip, port));
            server.InitServer();
            return server.Id;
        }
        public static bool GetServer (string serverId, out ServerInfo server, out string error)
        {
            if (_ServerList.TryGetValue(serverId, out server))
            {
                error = null;
                return true;
            }
            error = "socket.server.not.find";
            return false;
        }

        internal static void ClientClose (IClient client)
        {
            if (_ServerList.TryGetValue(client.ServerId, out ServerInfo server))
            {
                server.RemoveClient(client);
            }
        }
        public static bool CheckClientIsUsable (string serverId)
        {
            return _ServerList.TryGetValue(serverId, out ServerInfo server) && server.CheckIsUsable();
        }

        public static bool CheckIsUsable (string serverId, out string error)
        {
            if (!GetServer(serverId, out ServerInfo server, out error))
            {
                return false;
            }
            else if (server.CheckIsUsable())
            {
                return true;
            }
            else if (!PageManage.Send(Page.GetPingPage(serverId), out string res, out error))
            {
                return false;
            }
            else if (res != "ok")
            {
                error = "socket.reply.no.ok";
                return false;
            }
            return true;
        }
        public static bool Ping (string serverId)
        {
            if (!PageManage.Send(Page.GetPingPage(serverId), out string res, out string error))
            {
                if (LogSystem.CheckIsRecord(LogGrade.Information))
                {
                    new ErrorLog(error, "发送ping包失败!", "socket", LogGrade.Information)
                    {
                        {"server",serverId }
                    }.Save();
                }
                return false;
            }
            return res != "ok";
        }
        public static bool Ping (string serverId, out TimeSpan ping, out string error)
        {
            DateTime send = DateTime.Now;
            if (!PageManage.Send(Page.GetPingPage(serverId), out string res, out error))
            {
                if (LogSystem.CheckIsRecord(LogGrade.Information))
                {
                    new ErrorLog(error, "发送ping包失败!", "socket", LogGrade.Information)
                    {
                        {"server",serverId }
                    }.Save();
                }
                ping = TimeSpan.Zero;
                return false;
            }
            ping = DateTime.Now - send;
            return res == "ok";
        }
        internal static int GetClientConNum (string serverId)
        {
            if (_ServerList.TryGetValue(serverId, out ServerInfo server))
            {
                return server.ClientNum;
            }
            return 0;
        }
    }
}
