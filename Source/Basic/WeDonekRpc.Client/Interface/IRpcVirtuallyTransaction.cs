namespace WeDonekRpc.Client.Interface
{
    internal interface IRpcVirtuallyTransaction : ICurTranState
    {
        void InitTran(string tranName);
    }
}
