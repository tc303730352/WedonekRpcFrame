namespace WeDonekRpc.Helper.Interface
{
    public interface IAutoTask
    {
        bool IsExec ( int now );
        void ExecuteTask ();
    }
}