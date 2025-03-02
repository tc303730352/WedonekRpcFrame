namespace WeDonekRpc.Modular
{
    public interface IRpcDirectShieIdService
    {
        bool CheckIsShieId (string dictate);
        void Init ();

        void Close ();
    }
}