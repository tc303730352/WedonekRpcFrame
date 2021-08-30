using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using SocketTcpClient.Enum;
using SocketTcpClient.Interface;

using RpcHelper;

namespace SocketTcpClient.Model
{
        internal class ServerInfo
        {
                public string Id
                {
                        get;
                        set;
                }
                public int ClientNum => this._ClientNum;

                public ServerInfo(string ip, int port)
                {
                        this._IpAddress = ip;
                        this._ServerPort = port;
                        this.Id = string.Join(":", ip, port);
                }
                private volatile bool _IsInit = false;
                public void InitServer()
                {
                        if (this._IsInit == false)
                        {
                                this._IsInit = true;
                                this._SendThread = new Timer(new TimerCallback(this._SendPage), null, 50, 50);
                                for (int i = 0; i < Config.SocketConfig.ClientNum; i++)
                                {
                                        this._AddClient();
                                }
                        }
                }
                public bool CheckIsUsable()
                {
                        if (this._ClientNum == 0)
                        {
                                return false;
                        }
                        else
                        {
                                bool isUsable = false;
                                if (this._Lock.GetReadLock())
                                {
                                        isUsable = this._ClientList.FirstOrDefault(a => a.ClientStatus == ClientStatus.链接成功 || a.ClientStatus == ClientStatus.等待发送) != null;
                                        this._Lock.ExitRead();
                                }
                                return isUsable;
                        }
                }
                private readonly string _IpAddress = null;

                private readonly int _ServerPort = 0;

                private readonly HashSet<Interface.IClient> _ClientList = new HashSet<Interface.IClient>();
                private int _ClientNum = 0;

                private readonly ReadWriteLockHelper _Lock = new ReadWriteLockHelper();

                private readonly ConcurrentQueue<DataPage> _DataQueue = new ConcurrentQueue<DataPage>();

                private Timer _SendThread = null;

                public void SendPage(DataPage page)
                {
                        if (!this.FindClient(out IClient client))
                        {
                                Manage.PageManage.SendError(page.DataId, "socket.con.error");
                                return;
                        }
                        else if (client == null || !client.Send(page))
                        {
                                this._DataQueue.Enqueue(page);
                        }
                }

                private bool _AddClient()
                {
                        IClient client = new Client.ClientInfo(this.Id);
                        if (client.ConnectServer(this._IpAddress, this._ServerPort))
                        {
                                if (this._Lock.GetWriteLock())
                                {
                                        this._ClientList.Add(client);
                                        this._ClientNum = this._ClientList.Count;
                                        this._Lock.ExitWrite();
                                }
                                return true;
                        }
                        else
                        {
                                return false;
                        }
                }
                internal void SendHeartbeat()
                {
                        Page page = Page.GetSysPage("Heartbeat", string.Empty);
                        IClient[] clients = null;
                        if (this._Lock.GetReadLock())
                        {
                                clients = this._ClientList.ToArray();
                                this._Lock.ExitRead();
                        }
                        int time = HeartbeatTimeHelper.HeartbeatTime - 180;
                        Array.ForEach(clients, a =>
                        {
                                if (this._ClientNum > Config.SocketConfig.ClientNum && a.LastTime <= time)
                                {
                                        a.CloseClientCon();
                                }
                                else if (a.ClientStatus == Enum.ClientStatus.等待发送)
                                {
                                        a.SendSystemPage(page);
                                }
                        });
                }


                private void _SendPage(object state)
                {
                        if (this._DataQueue.Count > 0)
                        {
                                if (this._DataQueue.TryDequeue(out DataPage page))
                                {
                                        if (page.ConTimeOut <= HeartbeatTimeHelper.HeartbeatTime)
                                        {
                                                if (!this.CheckIsUsable())
                                                {
                                                        Manage.PageManage.SendError(page.DataId, "socket.con.error");
                                                }
                                                else
                                                {
                                                        Manage.PageManage.SendError(page.DataId, "socket.send.queue.over.time");
                                                }
                                                return;
                                        }
                                        this.SendPage(page);
                                }
                        }
                }

                internal void CloseClient()
                {
                        if (this._Lock.GetReadLock())
                        {
                                foreach (Interface.IClient i in this._ClientList)
                                {
                                        i.CloseClientCon();
                                }
                                this._Lock.ExitRead();
                        }
                }
                internal void RemoveClient(IClient client)
                {
                        if (this._Lock.GetWriteLock())
                        {
                                this._ClientList.Remove(client);
                                this._ClientNum = this._ClientList.Count;
                                this._Lock.ExitWrite();
                        }
                        if (Config.SocketConfig.SocketEvent != null && this._ClientList.Count == 0)
                        {
                                Config.SocketConfig.SocketEvent.ServerConClose();
                        }
                }
                internal bool FindClient(out IClient client)
                {
                        IClient c = null;
                        if (this._Lock.GetReadLock())
                        {
                                c = this._ClientList.FirstOrDefault(a => a.CheckIsSend());
                                this._Lock.ExitRead();
                        }
                        if (c == null)
                        {
                                client = null;
                                return this._AddClient();
                        }
                        client = c;
                        return true;
                }
        }
}
