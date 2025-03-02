namespace WeDonekRpc.Modular
{
    public interface IAccreditController
    {
        string AccreditId { get; }
        void ClearAccredit ();
        void SetAccreditId (string accreditId);
    }
}
