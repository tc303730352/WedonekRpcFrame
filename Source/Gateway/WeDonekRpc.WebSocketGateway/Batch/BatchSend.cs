using System;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpWebSocket.Interface;
using WeDonekRpc.WebSocketGateway.Interface;
using WeDonekRpc.WebSocketGateway.Model;

namespace WeDonekRpc.WebSocketGateway.Batch
{
    internal class BatchSend : BatchBasic, IBatchSend
    {
        private readonly Func<ISessionBody, bool> _Func = null;
        public BatchSend (Func<ISessionBody, bool> func, IService service, IResponseTemplate template) : base(service, template)
        {
            this._Func = func;
        }

        public int Send (string direct, object result)
        {
            IClientSession[] session = this._Service.FindSession(this._Func);
            if (session.Length == 0)
            {
                return 0;
            }
            string text = this._Template.GetResponse(new UserPage(direct), result);
            return this._Send(session, text);
        }
        public int Send (string direct, ErrorException error)
        {
            IClientSession[] session = this._Service.FindSession(this._Func);
            if (session.Length == 0)
            {
                return 0;
            }
            string text = this._Template.GetErrorResponse(new UserPage(direct), error);
            return this._Send(session, text);
        }
        public int Send (ErrorException error)
        {
            IClientSession[] session = this._Service.FindSession(this._Func);
            if (session.Length == 0)
            {
                return 0;
            }
            string text = this._Template.GetErrorResponse(error);
            return this._Send(session, text);
        }
    }
}
