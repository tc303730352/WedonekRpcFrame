namespace WeDonekRpc.Client.Interface
{
    public interface IRpcLocalTran
    {
        void BeginTran(ICurTran state);

        void Commint(ICurTran state);

        void RollBack(ICurTran state);
    }
}
