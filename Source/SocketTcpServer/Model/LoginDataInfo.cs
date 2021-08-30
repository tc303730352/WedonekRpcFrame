namespace SocketTcpServer.Model
{
        internal class LoginDataInfo
        {
                public LoginDataInfo()
                {
                }
                public LoginDataInfo(string key, string[] arg)
                {
                        this.LoginKey = key;
                        this.Arg = arg;
                }
                /// <summary>
                /// 登陆的密钥
                /// </summary>
                public string LoginKey
                {
                        get;
                        set;
                }

                /// <summary>
                /// 参数
                /// </summary>
                public string[] Arg
                {
                        get;
                        set;
                }
        }
}
