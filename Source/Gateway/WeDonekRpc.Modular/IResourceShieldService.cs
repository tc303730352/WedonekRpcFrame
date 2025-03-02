namespace WeDonekRpc.Modular
{
    public interface IResourceShieldService : System.IDisposable
    {
        void Init ();
        bool CheckIsShieId (string path);
    }
}