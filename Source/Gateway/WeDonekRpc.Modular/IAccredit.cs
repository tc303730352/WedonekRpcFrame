namespace WeDonekRpc.Modular
{
    public interface IAccredit
    {
        IUserState CurrentUser { get; }

        void CancelAccredit (string accreditId);
        IUserState SetCurrentAccredit (string accreditId);

        void SetAccreditId (string accreditId);
        void ClearAccredit ();
        IUserState GetAccredit (string accreditId);
    }
}