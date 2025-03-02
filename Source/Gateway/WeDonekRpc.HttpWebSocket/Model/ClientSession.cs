using System;
using System.IO;

using WeDonekRpc.HttpWebSocket.Interface;

using WeDonekRpc.Helper;

namespace WeDonekRpc.HttpWebSocket.Model
{
    internal class ClientSession : ISession
    {
        private readonly IWebSocketClient _Client = null;

        public ClientSession (IWebSocketClient client)
        {
            this._Client = client;
            this.SessionId = Guid.NewGuid();
        }

        public Guid SessionId
        {
            get;
        }
        private volatile bool _IsOnline = false;

        public bool IsOnline => this._IsOnline;

        public int OfflineTime
        {
            get;
            private set;
        }


        public string AccreditId
        {
            get;
            private set;
        }

        public string IdentityId
        {
            get;
            private set;
        }

        public bool IsAccredit
        {
            get;
            private set;
        }

        public Action<ISession> AccreditEvent { get; set; }

        public Action<ISession, string> CancelEvent { get; set; }

        public IWebSocketClient Client => this._Client;

        public string Name { get; private set; }

        public void Offline ()
        {
            this.OfflineTime = HeartbeatTimeHelper.HeartbeatTime;
            this._IsOnline = false;
        }


        public void Accredit (string accreditId, string identityId)
        {
            if (!this.IsAccredit)
            {
                this.AccreditId = accreditId;
                this.IdentityId = identityId;
            }
        }
        public void Accredit ()
        {
            this.IsAccredit = true;
            this.AccreditEvent(this);
        }

        public void CancelAccredit (string error)
        {
            if (this.IsAccredit)
            {
                this.IsAccredit = false;
                this.CancelEvent(this, error);
                this._Client.CloseClient(10);
            }
        }
        public bool Send (Stream stream)
        {
            return this._Client.Send(stream);

        }
        public bool Send (string text)
        {
            return this._Client.Send(text);
        }

        public void SetName (string name)
        {
            this.Name = name;
        }
    }
}
