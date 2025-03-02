namespace RpcStore.RemoteModel.TransmitScheme.Model
{
    public class TransmitSchemeDetailed : TransmitSchemeData
    {
        public TransmitDatum[] Transmits { get; set; }

        public string VerNumStr { get; set; }
        public string SystemType { get; set; }
    }
}
