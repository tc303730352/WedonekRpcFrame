namespace WeDonekRpc.TcpClient.Model
{
        internal class ServerConConfig
        {
                public ServerConConfig(string key, string[] arg)
                {
                        this.ConKey = key;
                        this.Arg = arg;
                }
                public string ConKey;
                public string[] Arg;
        }
}
