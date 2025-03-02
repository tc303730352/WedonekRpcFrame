using WeDonekRpc.Model;

namespace RpcStore.Collect.Model
{
    public class Transmit
    {
        public string ServerCode { get; set; }

        public string ServerName { get; set; }
        public List<TransmitRange> Range { get; set; }
    }
}
