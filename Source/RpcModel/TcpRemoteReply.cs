namespace RpcModel
{
        public class TcpRemoteReply
        {
                public TcpRemoteReply()
                {

                }

                public TcpRemoteReply(IBasicRes data)
                {
                        this.MsgBody = RpcHelper.Tools.Json(data);
                }
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
