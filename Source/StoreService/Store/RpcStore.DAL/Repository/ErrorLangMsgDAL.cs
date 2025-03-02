using RpcStore.Model.DB;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.IdGenerator;
using WeDonekRpc.SqlSugar;

namespace RpcStore.DAL.Repository
{
    internal class ErrorLangMsgDAL : IErrorLangMsgDAL
    {
        public class ErrorLang
        {
            public string ErrorCode { get; set; }

            public string Msg { get; set; }
        }
        private readonly IRepository<ErrorLangMsgModel> _BasicDAL;
        public ErrorLangMsgDAL (IRepository<ErrorLangMsgModel> dal)
        {
            this._BasicDAL = dal;
        }
        public long GetErrorLangId (long errorId, string lang)
        {
            return this._BasicDAL.Get(c => c.ErrorId == errorId && c.Lang == lang, c => c.Id);
        }

        public ErrorLangMsgModel[] GetErrorMsg (long[] errorId)
        {
            return this._BasicDAL.Gets(c => errorId.Contains(c.ErrorId));
        }
        public Dictionary<string, string> GetErrorDic (string[] error, string lang)
        {
            ErrorLang[] datas = this._BasicDAL.Join<ErrorCollectModel, ErrorLang>((a, b) => b.Id == a.ErrorId, (a, b) => error.Contains(b.ErrorCode) && a.Lang == lang, (a, b) => new ErrorLang
            {
                ErrorCode = b.ErrorCode,
                Msg = a.Msg
            });
            return datas.ToDictionary(c => c.ErrorCode, c => c.Msg);
        }
        public ErrorLangMsgModel[] GetErrorMsg (long errorId)
        {
            return this._BasicDAL.Gets(c => c.ErrorId == errorId);
        }
        public void Delete (long errorId)
        {
            ISqlQueue<ErrorLangMsgModel> queue = this._BasicDAL.BeginQueue();
            queue.Delete(a => a.ErrorId == errorId);
            queue.Update<ErrorCollectModel>(c => c.IsPerfect == false, c => c.Id == errorId);
            if (queue.Submit() <= 0)
            {
                throw new ErrorException("rpc.store.error.lang.drop.error");
            }
        }
        public void SetErrorMsg (long id, string msg)
        {
            if (!this._BasicDAL.Update(c => c.Msg == msg, c => c.Id == id))
            {
                throw new ErrorException("rpc.store.error.lang.set.error");
            }
        }
        public void AddErrorMsg (ErrorLangMsgModel add)
        {
            add.Id = IdentityHelper.CreateId();
            ISqlQueue<ErrorLangMsgModel> queue = this._BasicDAL.BeginQueue();
            queue.Insert(add);
            queue.Update<ErrorCollectModel>(c => c.IsPerfect == true, c => c.Id == add.ErrorId);
            if (queue.Submit() <= 0)
            {
                throw new ErrorException("rpc.store.error.lang.add.error");
            }
        }
        public void SyncErrorMsg (long errorId, Dictionary<string, string> lang)
        {
            ISqlQueue<ErrorLangMsgModel> queue = this._BasicDAL.BeginQueue();
            queue.Delete(a => a.ErrorId == errorId);
            queue.Insert(lang.ConvertAll(c => new ErrorLangMsgModel
            {
                ErrorId = errorId,
                Lang = c.Key,
                Msg = c.Value
            }));
            if (queue.Submit() <= 0)
            {
                throw new ErrorException("rpc.store.error.lang.set.error");
            }
        }
        public void AddErrorMsg (long errorId, Dictionary<string, string> lang)
        {
            ISqlQueue<ErrorLangMsgModel> queue = this._BasicDAL.BeginQueue();
            queue.Insert(lang.ConvertAll(c => new ErrorLangMsgModel
            {
                ErrorId = errorId,
                Lang = c.Key,
                Msg = c.Value
            }));
            queue.Update<ErrorCollectModel>(c => c.IsPerfect == true, c => c.Id == errorId);
            if (queue.Submit() <= 0)
            {
                throw new ErrorException("rpc.store.error.lang.set.error");
            }
        }
        public string GetErrorMsgText (long errorId, string lang)
        {
            return this._BasicDAL.Get(c => c.ErrorId == errorId && c.Lang == lang, c => c.Msg);
        }

        public ErrorLangMsgModel GetErrorMsg (long errorId, string lang)
        {
            return this._BasicDAL.Get(c => c.ErrorId == errorId && c.Lang == lang);
        }

    }
}
