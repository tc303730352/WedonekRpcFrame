namespace SocketTcpClient.Model
{
        internal class ServerConConfig
        {
                public ServerConConfig(string key, string[] arg)
                {
                        this.ConKey = key;
                        this.Arg = arg;
                }
                public string ConKey
                {
                        get;
                        set;
                }
                public string[] Arg
                {
                        get;
                        set;
                }
        }
}
