namespace WeDonekRpc.Helper.Error
{
    public interface ILocalErrorManage
    {
        ErrorConfig Config { get; }
        void TriggerDrop (IErrorManage error, ErrorMsg msg);
        bool RemoteGet (IErrorManage error, string code, out ErrorMsg msg);
    }
}