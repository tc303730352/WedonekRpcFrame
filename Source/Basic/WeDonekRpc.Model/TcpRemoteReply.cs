using WeDonekRpc.Helper.Json;

namespace WeDonekRpc.Model
{
    public class TcpRemoteReply
    {
        public TcpRemoteReply ()
        {

        }
        public TcpRemoteReply (IBasicRes data)
        {
            this.IsError = data.IsError;
            this.MsgBody = JsonTools.Json(data, data.GetType());
        }
        public TcpRemoteReply (string body)
        {
            this.IsError = false;
            this.MsgBody = body;
        }
        public bool IsError { get; set; }
        /// <summary>
        /// 消息体
        /// </summary>
        public string MsgBody
        {
            get;
            set;
        }
    }
}
