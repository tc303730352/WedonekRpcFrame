using WeDonekRpc.Helper.Json;

namespace WeDonekRpc.Model
{
    public class IRemoteResult : BasicRes
    {
        public IRemoteResult (long serverId, string error) : base(error)
        {
            this.RemoteServerId = serverId;
        }
        public IRemoteResult (string error) : base(error)
        {
        }

        public IRemoteResult (long serverId, TcpRemoteReply reply, bool isReply)
        {
            this._ResultStr = reply == null ? string.Empty : reply.MsgBody;
            this.RemoteServerId = serverId;
            this.IsReply = isReply;
        }
        public bool IsReply
        {
            get;
        }
        private readonly string _ResultStr = null;

        /// <summary>
        /// 回复的远程服务器Id
        /// </summary>
        public long RemoteServerId { get; private set; }


        public T GetResult<T> () where T : IBasicRes
        {
            return this._ResultStr != null ? JsonTools.Json<T>(this._ResultStr) : default;
        }

        public string GetResult ()
        {
            return this._ResultStr;
        }
    }
}
