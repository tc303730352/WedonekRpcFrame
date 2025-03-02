using WeDonekRpc.Helper;
using WeDonekRpc.Helper.IdGenerator;
using WeDonekRpc.Helper.Lock;
using RpcSync.Model;
using RpcSync.Model.DB;
using WeDonekRpc.SqlSugar;

namespace RpcSync.DAL.Repository
{
    internal class ErrorCodeDAL : IErrorCodeDAL
    {
        private readonly IRepository<ErrorCollectModel> _BasicDAL;
        public ErrorCodeDAL (IRepository<ErrorCollectModel> dal)
        {
            this._BasicDAL = dal;
        }
        public ErrorDatum SyncError (string code)
        {
            using (DataSyncLock syncLock = SyncLockFactory.ApplyLock(code))
            {
                if (syncLock.GetLock())
                {
                    ErrorDatum error = this._SyncError(code);
                    syncLock.Exit(error);
                    return error;
                }
                else if (syncLock.Result != null)
                {
                    return (ErrorDatum)syncLock.Result;
                }
                else
                {
                    throw new ErrorException("error.sync.fail");
                }
            }
        }
        private ErrorDatum _SyncError (string code)
        {
            ErrorDatum error = this._BasicDAL.Get(c => c.ErrorCode == code, c => new ErrorDatum
            {
                ErrorId = c.Id,
                IsPerfect = c.IsPerfect
            });
            if (error == null)
            {
                long id = IdentityHelper.CreateId();
                this._BasicDAL.Insert(new ErrorCollectModel
                {
                    Id = id,
                    ErrorCode = code,
                    IsPerfect = false
                });
                return new ErrorDatum { ErrorId = id, IsPerfect = false };
            }
            return error;
        }
        public string FindErrorCode (long errorId)
        {
            return this._BasicDAL.Get(a => a.Id == errorId, a => a.ErrorCode);
        }

        public long GetErroMaxId ()
        {
            return this._BasicDAL.Max<long>(a => a.Id);
        }
    }
}
