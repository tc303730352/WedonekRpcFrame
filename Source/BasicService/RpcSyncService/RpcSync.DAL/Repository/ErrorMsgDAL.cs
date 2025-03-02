using RpcSync.Model.DB;
using WeDonekRpc.SqlSugar;

namespace RpcSync.DAL.Repository
{
    internal class ErrorMsgDAL : IErrorMsgDAL
    {
        private readonly IRepository<ErrorLangMsgModel> _BasicDAL;
        public ErrorMsgDAL (IRepository<ErrorLangMsgModel> dal)
        {
            this._BasicDAL = dal;
        }

        public string GetErrorMsg (long errorId, string lang)
        {
            return this._BasicDAL.Get(a => a.ErrorId == errorId && a.Lang == lang, a => a.Msg);
        }

    }
}
