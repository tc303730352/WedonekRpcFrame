namespace WeDonekRpc.TcpServer.FileUp.Model
{
        internal class UpConfig
        {
                /// <summary>
                /// 上传限速（KB）
                /// </summary>
                public int LimitUpSpeed
                {
                        get;
                        set;
                }
                /// <summary>
                /// 上传超时时间
                /// </summary>
                public int UpTimeOut
                {
                        get;
                        set;
                }
        }
}
