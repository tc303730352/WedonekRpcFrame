using System;
using System.IO;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpWebSocket.Interface;
using WeDonekRpc.WebSocketGateway.Interface;

namespace WeDonekRpc.WebSocketGateway.Model
{
    internal class ClientSession : ISession
    {
        private readonly IClientSession _Session;
        private readonly IResponseTemplate _Template = null;
        public ClientSession (IClientSession session, IApiModular modular)
        {
            this._Template = modular.Config.ResponseTemplate;
            this._Session = session;
        }

        public bool IsOnline => this._Session.IsOnline;

        public int OfflineTime => this._Session.OfflineTime;

        public Guid SessionId => this._Session.SessionId;

        public string AccreditId => this._Session.AccreditId;

        public string IdentityId => this._Session.IdentityId;

        public bool IsAccredit => this._Session.IsAccredit;

        public string Name => this._Session.Name;

        public void CancelAccredit (string error)
        {
            this._Session.CancelAccredit(error);
        }

        public void Send (string direct, object data)
        {
            string text = this._Template.GetResponse(new UserPage(direct), data);
            _ = this._Session.Send(text);
        }
        public void Send (string direct, ErrorException error)
        {
            string text = this._Template.GetErrorResponse(new UserPage(direct), error);
            _ = this._Session.Send(text);
        }
        public void Send (ErrorException error)
        {
            string text = this._Template.GetErrorResponse(error);
            _ = this._Session.Send(text);
        }
        public void Send (Stream stream)
        {
            _ = this._Session.Send(stream);
        }

        public void SetName (string name)
        {
            this._Session.SetName(name);
        }
    }
}
