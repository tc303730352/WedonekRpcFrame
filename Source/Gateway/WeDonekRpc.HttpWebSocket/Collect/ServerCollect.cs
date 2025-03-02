using WeDonekRpc.HttpWebSocket.Interface;
using WeDonekRpc.HttpWebSocket.Model;
using WeDonekRpc.HttpWebSocket.Server;
using WeDonekRpc.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace WeDonekRpc.HttpWebSocket.Collect
{
    internal class ServerCollect
    {
        private static readonly Dictionary<Guid, ISocketServer> _ServerList = new Dictionary<Guid, ISocketServer>();

        private static readonly Timer _SyncTimer;
        static ServerCollect()
        {
            _SyncTimer = new Timer(_Sync, null, 60000, 60000);
        }

        private static void _Sync(object state)
        {
            if (_ServerList.Count == 0)
            {
                return;
            }
            Guid[] ids = _ServerList.Keys.ToArray();
            ids.ForEach(a =>
            {
                if (_ServerList.TryGetValue(a, out ISocketServer server))
                {
                    server.Sync();
                }
            });
        }

        private static ISocketServer _DefServer = null;


        public static IService AddServer(IWebSocketConfig config)
        {
            ISocketServer server = new SocketServer(config);
            _ServerList.Add(server.Id, server);
            if (_DefServer == null)
            {
                _DefServer = server;
            }
            return new BasicServer(server);
        }



        public static bool GetServer(Guid id, out ISocketServer server)
        {
            if (_DefServer.Id == id)
            {
                server = _DefServer;
                return true;
            }
            return _ServerList.TryGetValue(id, out server);
        }


        internal static bool CheckIsOnline(Guid serverId, Guid clientId)
        {
            if (!GetServer(serverId, out ISocketServer server))
            {
                return false;
            }
            return server.CheckIsOnline(clientId);
        }

        internal static void CloseCon(ClientDatum client, int time)
        {
            if (GetServer(client.ServerId, out ISocketServer server))
            {
                server.CloseCon(client.ClientId, time);
            }
        }

        internal static void CloseCon(ClientDatum client)
        {
            if (GetServer(client.ServerId, out ISocketServer server))
            {
                server.CloseCon(client.ClientId);
            }
        }
    }
}
