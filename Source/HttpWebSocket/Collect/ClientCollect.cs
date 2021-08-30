using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

using HttpWebSocket.Client;
using HttpWebSocket.Interface;
using HttpWebSocket.Server;

namespace HttpWebSocket.Collect
{
        internal class ClientCollect
        {
                private readonly ISocketServer _Server = null;
                public ClientCollect(ISocketServer server)
                {
                        this._Server = server;
                        this._HeartbeatTimer = new Timer(new TimerCallback(this._SendHeartbeat), null, 100, 1000);
                }

                private readonly ConcurrentDictionary<Guid, IWebSocketClient> _ClientList = new ConcurrentDictionary<Guid, IWebSocketClient>();
                private readonly Timer _HeartbeatTimer = null;

                private void _SendHeartbeat(object state)
                {
                        if (this._ClientList.IsEmpty)
                        {
                                return;
                        }
                        Guid[] clients = this._ClientList.Keys.ToArray();
                        int time = RpcHelper.HeartbeatTimeHelper.HeartbeatTime - this._Server.Config.HeartbeatTime;
                        Parallel.ForEach(clients, a =>
                        {
                                if (this._ClientList.TryGetValue(a, out IWebSocketClient client) && client.IsCon && client.HeartbeatTime <= time)
                                {
                                        if (client.CheckClient())
                                        {
                                                _Server.CheckSessionState(client);
                                        }
                                }
                        });
                }

                internal void AddClient(Socket client)
                {
                        IWebSocketClient add = new WebSocketClient(client, this._Server);
                        if (this._ClientList.TryAdd(add.ClientId, add))
                        {
                                add.InitSocket();
                        }
                }
                public bool GetClient(Guid id, out IWebSocketClient client)
                {
                        return this._ClientList.TryGetValue(id, out client);
                }
                internal bool RemoveClient(Guid id,out IWebSocketClient client)
                {
                        if (this._ClientList.TryRemove(id, out client))
                        {
                                client.Dispose();
                                return true;
                        }
                        return false;
                }
        }
}
