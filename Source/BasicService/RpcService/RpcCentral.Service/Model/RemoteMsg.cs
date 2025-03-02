using WeDonekRpc.Helper.Json;

namespace RpcCentral.Service.Model
{
    public class RemoteMsg
    {
        public RemoteMsg (string msg, string clientIp)
        {
            this.ClientIp = clientIp;
            this._MsgBody = msg;
        }

        public string ClientIp { get; }

        private readonly string _MsgBody = null;

        public string GetMsgBody ()
        {
            return this._MsgBody;
        }
        public T GetMsgBody<T> ()
        {
            return JsonTools.Json<T>(this._MsgBody);
        }

    }
}
