using System;
using System.Collections.Concurrent;
using System.Linq;

using SocketTcpServer.Client;
using SocketTcpServer.Interface;

using RpcHelper;

namespace SocketTcpServer.Manage
{
        internal class ClientManage
        {
                /// <summary>
                /// 连接的客户端列表
                /// </summary>
                private readonly ConcurrentDictionary<Guid, IClient> _ClientList = new ConcurrentDictionary<Guid, IClient>();

                internal int ClientCount => this._ClientList.Count;

                /// <summary>
                /// 处理数据包的接口
                /// </summary>
                private readonly IAllot objAllot = null;

                private readonly ISocketEvent _SocketEvent = null;

                internal ClientManage(IAllot allot, ISocketEvent sevent)
                {
                        this.objAllot = allot;
                        this._SocketEvent = sevent;
                }
                internal ClientManage()
                {
                }

                /// <summary>
                /// 添加一个客户端
                /// </summary>
                /// <param name="client"></param>
                internal void AddClient(Guid serverId, Client.SocketClient client)
                {
                        if (this.objAllot != null)
                        {
                                this._ClientList.TryAdd(client.ClientId, new ClientInfo(serverId, client, this.objAllot, this._SocketEvent));
                        }
                }

                /// <summary>
                /// 客户端关闭事件
                /// </summary>
                /// <param name="clientId"></param>
                internal void ClientCloseEvent(Guid clientId)
                {
                        if (this._ClientList.TryRemove(clientId, out IClient client))
                        {
                                if (this._SocketEvent != null)
                                {
                                        this._SocketEvent.ClientConnectClose(clientId, client.BindParam);
                                }
                        }
                }
                /// <summary>
                /// 向客户端发送数据包
                /// </summary>
                /// <param name="type"></param>
                /// <param name="sendType"></param>
                /// <param name="content"></param>
                internal void Send(Guid clientId, Model.DataPage page)
                {
                        if (this._ClientList.TryGetValue(clientId, out IClient client))
                        {
                                client.Send(page);
                        }
                }
                internal void CheckIsCon()
                {
                        IClient[] clientList = this._ClientList.Values.ToArray();
                        if (clientList.Length != 0)
                        {
                                DateTime time = DateTime.Now.AddSeconds(-5);
                                clientList.ForEach(a => a.CheckIsCon(time));
                        }
                }

                internal IClient[] GetClient(params Guid[] clientId)
                {
                        return this._ClientList.Values.Where(a => clientId.Count(b => b == a.ClientId) > 0 && a.CurrentStatus != Enum.ClientStatus.已关闭).ToArray();
                }
                internal bool GetClient(Guid clientId, out IClient client)
                {
                        return this._ClientList.TryGetValue(clientId, out client);
                }
                internal bool GetClient(Guid clientId, out IClient client, out string error)
                {
                        if (!this._ClientList.TryGetValue(clientId, out client))
                        {
                                error = "socket.client.not.find";
                                return false;
                        }
                        else if (client.CurrentStatus != Enum.ClientStatus.已关闭)
                        {
                                error = "socket.client.con.close";
                                return false;
                        }
                        else
                        {
                                error = null;
                                return true;
                        }
                }
                internal void CloseClient(Guid clientId)
                {
                        if (this._ClientList.TryGetValue(clientId, out IClient client))
                        {
                                client.CloseClientCon(2);
                        }
                }

                internal void CloseClient()
                {
                        Interface.IClient[] clientList = this._ClientList.Values.ToArray();
                        foreach (ClientInfo i in clientList)
                        {
                                i.CloseClientCon(2);
                        }
                }
        }
}
