using System;
using System.Collections.Generic;
using System.Linq;

using HttpWebSocket.Interface;
using HttpWebSocket.Model;
using HttpWebSocket.Server;

using RpcHelper;
using RpcHelper.TaskTools;

namespace HttpWebSocket.Collect
{
        internal class ServerCollect
        {
                private static readonly Dictionary<Guid, ISocketServer> _ServerList = new Dictionary<Guid, ISocketServer>();

                static ServerCollect()
                {
                        TaskManage.AddTask(new TaskHelper("同步WebSocket服务", new TimeSpan(0, 1, 0), new Action(_Sync)));
                }

                private static void _Sync()
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
