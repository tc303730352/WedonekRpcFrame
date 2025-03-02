using RpcSync.DAL;
using RpcSync.Model;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Error;

namespace RpcSync.Collect.Collect
{
    internal class ErrorCollect : IErrorCollect
    {
        private readonly IErrorCodeDAL _ErrorCode;

        private readonly IErrorMsgDAL _ErrorMsg;

        private readonly ICacheController _Cache;

        public ErrorCollect ( IErrorCodeDAL errorCode,
            IErrorMsgDAL errorMsg,
            ICacheController cache )
        {
            this._ErrorCode = errorCode;
            this._ErrorMsg = errorMsg;
            this._Cache = cache;
        }
        public void Refresh ( long errorId )
        {
            string code = this.FindErrorCode(errorId);
            string key = string.Concat("Error_", code);
            _ = this._Cache.Remove(key);
            key = string.Concat("ErrorDatum_", code);
            _ = this._Cache.Remove(key);
        }
        public string FindErrorCode ( long errorId )
        {
            string key = string.Concat("ErrorCode_", errorId);
            if ( this._Cache.TryGet(key, out string code) )
            {
                return code;
            }
            code = this._ErrorCode.FindErrorCode(errorId);
            if ( code == null )
            {
                throw new ErrorException("error.code.not.find");
            }
            _ = this._Cache.Set(key, code, new TimeSpan(30, 0, 0, 0));
            return code;
        }
        public ErrorMsg FindError ( string code, string lang )
        {
            string key = string.Concat("Error_", code);
            if ( this._Cache.TryGet(key, out ErrorMsg msg) )
            {
                return msg;
            }
            msg = this._GetError(code, lang);
            _ = this._Cache.Set(key, msg, new TimeSpan(5, 0, 0, 0));
            return msg;
        }
        private ErrorMsg _GetError ( string code, string lang )
        {
            ErrorDatum error = this._GetError(code);
            if ( !error.IsPerfect )
            {
                return new ErrorMsg
                {
                    ErrorCode = code,
                    ErrorId = error.ErrorId,
                    Lang = lang
                };
            }
            string msg = this._ErrorMsg.GetErrorMsg(error.ErrorId, lang);
            return new ErrorMsg
            {
                ErrorCode = code,
                ErrorId = error.ErrorId,
                Lang = lang,
                Text = msg
            };
        }
        private ErrorDatum _GetError ( string code )
        {
            string key = string.Concat("ErrorDatum_", code);
            if ( this._Cache.TryGet(key, out ErrorDatum datum) )
            {
                return datum;
            }
            datum = this._ErrorCode.SyncError(code);
            _ = this._Cache.Set(key, datum, new TimeSpan(10, 0, 0, 0));
            return datum;
        }

        public long GetOrAdd ( string code )
        {
            ErrorDatum datum = this._GetError(code);
            return datum.ErrorId;
        }
    }
}
