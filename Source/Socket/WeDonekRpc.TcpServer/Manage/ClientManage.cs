using System;
using System.Collections.Concurrent;
using System.Linq;
using WeDonekRpc.Helper;
using WeDonekRpc.TcpServer.Client;
using WeDonekRpc.TcpServer.Interface;

namespace WeDonekRpc.TcpServer.Manage
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

        internal ClientManage (IAllot allot, ISocketEvent sevent)
        {
            this.objAllot = allot;
            this._SocketEvent = sevent;
        }
        internal ClientManage ()
        {
        }

        /// <summary>
        /// 添加一个客户端
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="client"></param>
        internal void AddClient (Guid serverId, Client.SocketClient client)
        {
            if (this.objAllot != null)
            {
                ClientInfo cli = new ClientInfo(serverId, client, this.objAllot, this._SocketEvent);
                if (this._ClientList.TryAdd(client.ClientId, cli))
                {
                    cli.Welcome();
                }
                else
                {
                    cli.CloseClientCon();
                }
            }
        }

        /// <summary>
        /// 客户端关闭事件
        /// </summary>
        /// <param name="clientId"></param>
        internal void ClientCloseEvent (Guid clientId)
        {
            if (this._ClientList.TryRemove(clientId, out IClient client))
            {
                this._SocketEvent?.ClientConnectClose(clientId, client.BindParam);
            }
        }

        internal void CheckIsCon ()
        {
            IClient[] clientList = this._ClientList.Values.ToArray();
            if (clientList.Length != 0)
            {
                DateTime time = DateTime.Now.AddSeconds(-5);
                clientList.ForEach(a => a.CheckIsCon(time));
            }
        }


        internal void CloseClient (Guid clientId)
        {
            if (this._ClientList.TryGetValue(clientId, out IClient client))
            {
                client.CloseClientCon(2);
            }
        }

        internal void CloseClient ()
        {
            Interface.IClient[] clientList = this._ClientList.Values.ToArray();
            foreach (ClientInfo i in clientList)
            {
                i.CloseClientCon(2);
            }
        }

        internal IClient Find (string bindParam)
        {
            return this._ClientList.Where(a => a.Value.BindParam == bindParam && a.Value.CurrentStatus == Enum.ClientStatus.已连接).Select(a => a.Value).FirstOrDefault();
        }
    }
}
