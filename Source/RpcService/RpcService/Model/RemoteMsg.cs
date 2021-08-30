namespace RpcService.Model
{
        internal class RemoteMsg
        {
                public RemoteMsg(string msg, string clientIp)
                {
                        this.ClientIp = clientIp;
                        this._MsgBody = msg;
                }

                public string ClientIp { get; }

                private readonly string _MsgBody = null;

                public string GetMsgBody()
                {
                        return this._MsgBody;
                }
                public T GetMsgBody<T>()
                {
                        return RpcHelper.Tools.Json<T>(this._MsgBody);
                }

        }
}
