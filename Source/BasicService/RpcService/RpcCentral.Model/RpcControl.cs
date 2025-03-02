namespace RpcCentral.Model
{
    public class RpcControl
    {
        public int Id { get; set; }
        public string ServerIp
        {
            get;
            set;
        }
        public int Port
        {
            get;
            set;
        }
        public int RegionId
        {
            get;
            set;
        }
    }
}
