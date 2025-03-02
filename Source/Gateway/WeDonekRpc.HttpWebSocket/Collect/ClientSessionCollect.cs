using WeDonekRpc.HttpWebSocket.Config;
using WeDonekRpc.HttpWebSocket.Interface;
using WeDonekRpc.HttpWebSocket.Model;
using WeDonekRpc.Helper;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

namespace WeDonekRpc.HttpWebSocket.Collect
{
    internal class ClientSessionCollect : IClientSessionCollect
    {
        private struct _SyncDic
        {
            public Guid SessionId;
            public string AccreditId;
        }
        /// <summary>
        /// 客户端唯一索引字典
        /// </summary>
        private readonly ConcurrentDictionary<Guid, ISession> _SessionList = new ConcurrentDictionary<Guid, ISession>();

        private readonly ConcurrentDictionary<string, Guid> _AccreditKeys;
        private readonly ConcurrentQueue<_SyncDic> _SyncQueue;
        private readonly Timer _CheckAccreditTimer;

        private readonly ISocketServer _Server;

        public ClientSessionCollect(ISocketServer server)
        {
            this._Server = server;
            if (server.Config.IsSingle)
            {
                this._AccreditKeys = new ConcurrentDictionary<string, Guid>();
                this._SyncQueue = new ConcurrentQueue<_SyncDic>();
                this._CheckAccreditTimer = new Timer(this._SyncAccredit, null, 1500, 1500);
            }
        }

        private void _SyncAccredit(object state)
        {
            if (this._SyncQueue.Count == 0)
            {
                return;
            }
            if (this._SyncQueue.TryDequeue(out _SyncDic data))
            {
                Guid old = Guid.Empty;
                if (this._AccreditKeys.AddOrUpdate(data.AccreditId, data.SessionId, (a, b) =>
                {
                    old = b;
                    return data.SessionId;
                }) == data.SessionId && old != Guid.Empty)
                {
                    this._Cancel(old, "websocket.accredit.cancel");
                }
            }
        }
        private void _Cancel(Guid sessionId, string error)
        {
            if (this._SessionList.TryGetValue(sessionId, out ISession client))
            {
                client.CancelAccredit(error);
            }
        }

        public void ClearSession()
        {
            if (this._SessionList.IsEmpty)
            {
                return;
            }
            int time = HeartbeatTimeHelper.HeartbeatTime - PublicConfig.OfflineLimit;
            Guid[] keys = this._SessionList.Where(a => !a.Value.IsOnline && a.Value.OfflineTime <= time).Select(a => a.Key).ToArray();
            if (keys.Length > 0)
            {
                keys.ForEach(a =>
                {
                    if (this._SessionList.TryRemove(a, out ISession session))
                    {
                        this._DropAccredit(session);
                    }
                });
            }
        }
        public ISession GetSession(Guid sessionId)
        {
            return this._SessionList.TryGetValue(sessionId, out ISession client) ? client : null;
        }
        public IClientSession[] GetSession(Guid[] sessionId)
        {
            return sessionId.Convert(a =>
            {
                return this._SessionList.TryGetValue(a, out ISession client) ? client : null;
            });
        }
        public IClientSession[] FindSession(Func<ISessionBody, bool> find)
        {
            return this._SessionList.Where(a => find(a.Value)).Select(a => a.Value).ToArray();
        }

        public ISession CreateSession(IWebSocketClient client)
        {
            ISession session = new ClientSession(client);
            if (!this._SessionList.TryAdd(session.SessionId, session))
            {
                LogSystem.AddErrorLog("会话创建失败!", string.Format("serverId:{0}\r\nclientId:{1}", this._Server.Id, client.ClientId), "web.socket.session.add.error");
                return null;
            }
            session.AccreditEvent = new Action<ISession>(this._Accredit);
            session.CancelEvent = new Action<ISession, string>(this._Cancel);
            return session;
        }
        private void _DropAccredit(ISession session)
        {
            if (!this._Server.Config.IsSingle && session.IsAccredit)
            {
                return;
            }
            else if (this._AccreditKeys.TryGetValue(session.AccreditId, out Guid sessionId) && sessionId == session.SessionId)
            {
                _ = this._AccreditKeys.TryRemove(session.AccreditId, out _);
            }
        }
        private void _Cancel(ISession obj, string error)
        {
            if (obj.IsOnline)
            {
                this._Server.ReplyError(obj.Client, new ErrorException(error), "SessionEnd");
            }
            this._DropAccredit(obj);
        }

        private void _Accredit(ISession obj)
        {
            if (!this._Server.Config.IsSingle && !obj.AccreditId.IsNull())
            {
                return;
            }
            this._SyncQueue.Enqueue(new _SyncDic
            {
                AccreditId = obj.AccreditId,
                SessionId = obj.SessionId
            });
        }

        public ISession Offline(Guid sessionId)
        {
            if (this._SessionList.TryGetValue(sessionId, out ISession session))
            {
                session.Offline();
                return session;
            }
            return null;
        }

        public void Cancel(string accreditId, string error)
        {
            if (error == null)
            {
                error = "websocket.accredit.cancel";
            }
            if (this._Server.Config.IsSingle)
            {
                if (this._AccreditKeys.TryGetValue(accreditId, out Guid sessionId))
                {
                    this._Cancel(sessionId, error);
                }
                return;
            }
            ISession[] session = this._SessionList.Where(a => a.Value.IsAccredit && a.Value.AccreditId == accreditId).Select(a => a.Value).ToArray();
            if (session.Length > 0)
            {
                session.ForEach(a => a.CancelAccredit(error));
            }
        }

        public IClientSession[] FindSession(string accreditId)
        {
            if (this._Server.Config.IsSingle)
            {
                if (this._AccreditKeys.TryGetValue(accreditId, out Guid sessionId))
                {
                    if (this._SessionList.TryGetValue(sessionId, out ISession session))
                    {
                        return new IClientSession[] { session };
                    }
                }
                return null;
            }
            return this._SessionList.Where(a => a.Value.IsAccredit && a.Value.AccreditId == accreditId).Select(a => a.Value).ToArray();
        }

        public IClientSession FindOnlineSession(string accreditId, string name)
        {
            return this._SessionList.Where(a => a.Value.IsAccredit && a.Value.AccreditId == accreditId && a.Value.Name == name && a.Value.IsOnline).Select(a => a.Value).FirstOrDefault();
        }
    }
}
