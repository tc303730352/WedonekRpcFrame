using System.Collections.Concurrent;

using RpcClient.Collect;
using RpcClient.Resource;

using RpcHelper;

namespace RpcClient.Controller
{

        internal class ErrorController : DataSyncClass
        {
                public ErrorController(string code)
                {
                        this.ErrorCode = code;
                }
                public string ErrorCode
                {
                        get;
                }
                public long ErrorId { get; private set; }

                private readonly ConcurrentDictionary<string, string> _Lang = new ConcurrentDictionary<string, string>();

                protected override bool SyncData()
                {
                        if (!ErrorRemote.GetErrorId(this.ErrorCode, out long errorId, out string error))
                        {
                                this.Error = error;
                                return false;
                        }
                        else
                        {
                                this.ErrorId = errorId;
                                ErrorCollect.SetError(errorId, this.ErrorCode);
                                return true;
                        }
                }

                private string _GetErrorMsg(string lang)
                {
                        if (!ErrorRemote.GetErrorMsg(lang, this.ErrorCode, out string msg, out _))
                        {
                                return this.ErrorCode;
                        }
                        else if (string.IsNullOrEmpty(msg))
                        {
                                msg = this.ErrorCode;
                        }
                        this._Lang.TryAdd(lang, msg);
                        return msg;
                }

                public ErrorMsg GetErrorMsg(string lang)
                {
                        if (!this._Lang.TryGetValue(lang, out string msg))
                        {
                                msg = this._GetErrorMsg(lang);
                        }
                        return new ErrorMsg
                        {
                                ErrorCode = ErrorCode,
                                ErrorId = ErrorId,
                                Lang = lang,
                                Msg = msg
                        };
                }
        }
}
