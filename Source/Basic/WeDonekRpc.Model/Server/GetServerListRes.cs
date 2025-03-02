namespace WeDonekRpc.Model.Server
{
    public class GetServerListRes
    {
        public BalancedType BalancedType
        {
            get;
            set;
        }
        public ServerConfig[] Servers
        {
            get;
            set;
        }
        public ServerConfig[] BackUp
        {
            get;
            set;
        }
        public string TransmitVer { get; set; }
        public int ConfigVer { get; set; }
    }
}
