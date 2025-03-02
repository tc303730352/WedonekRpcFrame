using RpcStore.Model.DB;

namespace RpcStore.Collect
{
    public interface IErrorLangMsgCollect
    {
        Dictionary<string, string> GetErrorDic (string[] error, string lang);
        ErrorLangMsgModel GetErrorMsg (long errorId, string lang);
        string GetErrorMsgText (long errorId, string lang);
        ErrorLangMsgModel[] GetErrorMsg (long[] errorId);
    }
}