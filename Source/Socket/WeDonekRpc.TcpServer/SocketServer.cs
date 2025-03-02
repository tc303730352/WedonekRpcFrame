using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Log;
using WeDonekRpc.IOBuffer;
using WeDonekRpc.TcpServer.Interface;
using WeDonekRpc.TcpServer.Server;
using WeDonekRpc.TcpServer.SystemAllot;

namespace WeDonekRpc.TcpServer
{
    public class TcpServer
    {
        static TcpServer ()
        {
            BufferCollect.InitBuffer();
            _ = new Timer(new TimerCallback(_CheckIsCon), null, 60000, 60000);
        }
        private static void _CheckIsCon (object state)
        {
            TcpServer.CheckClientIsCon();
        }


        private static readonly Dictionary<Guid, Server.ServerInfo> serverList = [];

        private static Guid _DefaultServerId = Guid.Empty;
        public static Guid DefaultServerId => _DefaultServerId;

        private static void InitSocketTcpServer ()
        {
            IAllot allot = Config.SocketConfig.DefaultAllot;
            if (allot == null)
            {
                allot = new UserAllotInfo();
            }
            _DefaultServerId = AddSocketTcpServer(Config.SocketConfig.DefaultServerPort, allot);
        }

        public static Guid GetServerId (int port)
        {
            return serverList.Where(a => a.Value.Port == port).Select(a => a.Key).FirstOrDefault();
        }
        /// <summary>
        /// 注册一个新的Socket服务
        /// </summary>
        /// <param name="port"></param>
        public static Guid AddSocketTcpServer (int port, Interface.IAllot allot)
        {
            if (allot == null)
            {
                return Guid.Empty;
            }
            Server.ServerInfo objServer = new Server.ServerInfo(allot, Config.SocketConfig.SocketEvent, Config.SocketConfig.ServerKey);
            objServer.InitServer(port);
            serverList.Add(objServer.ServerId, objServer);
            return objServer.ServerId;
        }
        public static Guid AddSocketTcpServer (int port, Interface.IAllot allot, ISocketEvent socketEvent)
        {
            return AddSocketTcpServer(port, allot, socketEvent, Config.SocketConfig.ServerKey);
        }
        /// <summary>
        /// 注册一个新的Socket服务
        /// </summary>
        /// <param name="port"></param>
        public static Guid AddSocketTcpServer (int port, Interface.IAllot allot, ISocketEvent socketEvent, string punlicKey)
        {
            if (allot == null)
            {
                return Guid.Empty;
            }
            punlicKey = Tools.GetMD5(punlicKey);
            Server.ServerInfo objServer = new Server.ServerInfo(allot, socketEvent, punlicKey);
            objServer.InitServer(port);
            serverList.Add(objServer.ServerId, objServer);
            return objServer.ServerId;
        }
        public static int GetSocketClientNum (Guid serverId)
        {
            if (serverList.ContainsKey(serverId))
            {
                return serverList[serverId].ClientCount;
            }
            return 0;
        }

        internal static bool CheckPublicKey (Guid serverId, string key)
        {
            if (serverList.TryGetValue(serverId, out Server.ServerInfo server))
            {
                return server.PublicKey == key;
            }
            return false;
        }

        public static void CloseClient (params Guid[] clientId)
        {
            if (clientId != null)
            {
                CloseClient(_DefaultServerId, clientId);
            }
        }
        public static void CloseClient (Guid serverId, params Guid[] clientId)
        {
            if (clientId != null)
            {
                if (serverList.TryGetValue(serverId, out ServerInfo server))
                {
                    foreach (Guid i in clientId)
                    {
                        server.CloseClient(i);
                    }
                }
            }
        }

        /// <summary>
        /// 客户端关闭事件
        /// </summary>
        /// <param name="clientId"></param>
        internal static void ClientCloseEvent (Guid serverId, Guid clientId)
        {
            if (serverList.TryGetValue(serverId, out ServerInfo server))
            {
                server.ClientCloseEvent(clientId);
            }
        }

        internal static void CloseClient (Guid serverId, Guid clientId)
        {
            if (serverList.TryGetValue(serverId, out ServerInfo server))
            {
                server.CloseClient(clientId);
            }
        }

        public static void CloseServer ()
        {
            foreach (Server.ServerInfo i in serverList.Values)
            {
                i.CloseServer();
            }
            LogSystem.CloseLog();
        }

        public static void CloseServer (int port)
        {
            Server.ServerInfo server = serverList.Values.Where(a => a.Port == port).FirstOrDefault();
            server?.CloseServer();
        }

        internal static void CheckClientIsCon ()
        {
            foreach (ServerInfo i in serverList.Values)
            {
                i.CheckIsCon();
            }
        }



        internal static bool GetServer (Guid serverId, out ServerInfo server)
        {
            return serverList.TryGetValue(serverId, out server);
        }
        internal static bool GetServer (Guid serverId, out ServerInfo server, out string error)
        {
            if (!serverList.TryGetValue(serverId, out server))
            {
                error = "socket.server.not.find";
                return false;
            }
            error = null;
            return true;
        }
        public static void Init ()
        {
            InitSocketTcpServer();
        }
    }
}
