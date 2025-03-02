using RpcStore.Model.DB;

namespace RpcStore.Model.TransmitScheme
{
    public class ServerTransmitScheme : ServerTransmitSchemeModel
    {
        /// <summary>
        /// 负载方案
        /// </summary>
        public TransmitDto[] Transmits { get; set; }
    }
}
