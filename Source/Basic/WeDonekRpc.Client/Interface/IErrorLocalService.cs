namespace WeDonekRpc.Client.Interface
{
    public interface IErrorLocalService
    {
        string GetCode(long errorId);
        long GetErrorId(string code);
        string GetText(string code, string def);
        string GetText(string code, string lang, string def);
    }
}