using RpcStore.Model.DB;

namespace RpcStore.DAL
{
    public interface IErrorLangMsgDAL
    {
        Dictionary<string, string> GetErrorDic (string[] error, string lang);
        void AddErrorMsg (ErrorLangMsgModel add);
        void SyncErrorMsg (long errorId, Dictionary<string, string> lang);
        void AddErrorMsg (long errorId, Dictionary<string, string> lang);
        void Delete (long errorId);
        long GetErrorLangId (long errorId, string lang);
        ErrorLangMsgModel[] GetErrorMsg (long errorId);
        string GetErrorMsgText (long errorId, string lang);
        ErrorLangMsgModel GetErrorMsg (long errorId, string lang);
        ErrorLangMsgModel[] GetErrorMsg (long[] errorId);
        void SetErrorMsg (long id, string msg);
    }
}