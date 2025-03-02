namespace WeDonekRpc.Client.Interface
{
    public interface IRpcPcTranService
    {
        void RegDbTran(string name, IRpcTwoPcTran dbTran);
        void RegDbTran<T>(string name) where T : IRpcTwoPcTran;
    }
}