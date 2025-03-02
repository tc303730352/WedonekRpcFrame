namespace WeDonekRpc.Modular
{
    public interface IAccreditConfig
    {
        int ErrorVaildTime { get; }
        int MaxCacheTime { get; }
        int MaxCheckime { get; }
        int MinCacheTime { get; }
        int MinCheckTime { get; }
        int RefreshTime { get; }
        string RoleType { get; }
        int GetCacheVaildTime ();
        int GetNextCheckTime (int time);
    }
}