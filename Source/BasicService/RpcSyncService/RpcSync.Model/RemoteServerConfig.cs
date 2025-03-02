namespace RpcSync.Model
{
    public class RemoteServerConfig
    {
        public long ServerId { get; set; }
        public string ServerName { get; set; }
        public string RemoteIp { get; set; }
        public int ServerPort { get; set; }
        public string ServerIp { get; set; }
        public string ConIp { get; set; }

        public string SystemType { get; set; }

        public string GroupName { get; set; }
    }
}
