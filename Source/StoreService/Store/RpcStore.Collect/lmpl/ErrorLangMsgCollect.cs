using RpcStore.DAL;
using RpcStore.Model.DB;

namespace RpcStore.Collect.lmpl
{
    internal class ErrorLangMsgCollect : IErrorLangMsgCollect
    {
        private readonly IErrorLangMsgDAL _BasicDAL;

        public ErrorLangMsgCollect (IErrorLangMsgDAL basicDAL)
        {
            this._BasicDAL = basicDAL;
        }

        public ErrorLangMsgModel[] GetErrorMsg (long[] errorId)
        {
            return this._BasicDAL.GetErrorMsg(errorId);
        }
        public string GetErrorMsgText (long errorId, string lang)
        {
            return this._BasicDAL.GetErrorMsgText(errorId, lang);
        }
        public ErrorLangMsgModel GetErrorMsg (long errorId, string lang)
        {
            return this._BasicDAL.GetErrorMsg(errorId, lang);
        }
        public Dictionary<string, string> GetErrorDic (string[] error, string lang)
        {
            return this._BasicDAL.GetErrorDic(error, lang);
        }
    }
}
