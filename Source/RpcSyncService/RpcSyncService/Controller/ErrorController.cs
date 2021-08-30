using System.Collections.Generic;

using RpcSyncService.Collect;
using RpcSyncService.Model;
using RpcSyncService.Model.DAL_Model;

using RpcHelper;
namespace RpcSyncService.Controller
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
                private readonly Dictionary<string, string> _ErrorMsg = new Dictionary<string, string>();

                private long _ErrorId = 0;
                private bool _IsPerfect = false;

                protected override bool SyncData()
                {
                        if (!ErrorCollect.FindError(this.ErrorCode, out ErrorDatum datum, out string error))
                        {
                                this.Error = error;
                                return false;
                        }
                        else if (!datum.IsPerfect)
                        {
                                ErrorCollect.SetMaxErrorId(datum.ErrorId);
                                this._ErrorId = datum.ErrorId;
                                return true;
                        }
                        else if (!ErrorLangCollect.GetErrorLang(datum.ErrorId, out ErrorLang[] lang, out error))
                        {
                                this.Error = error;
                                return false;
                        }
                        else
                        {
                                ErrorCollect.SetMaxErrorId(datum.ErrorId);
                                lang.ForEach(a =>
                                {
                                        this._ErrorMsg.Add(a.Lang, a.Msg);
                                });
                                this._ErrorId = datum.ErrorId;
                                this._IsPerfect = datum.IsPerfect;
                        }
                        return true;
                }

                public ErrorMsg GetError(string lang)
                {
                        if (!this._IsPerfect)
                        {
                                return new ErrorMsg
                                {
                                        ErrorId = this._ErrorId,
                                        Lang = lang,
                                        ErrorCode = this.ErrorCode,
                                        Msg = RpcClient.RpcClient.Config.GetConfigVal(string.Format("DefErrorMsg_{0}", lang))
                                };
                        }
                        else
                        {
                                if (!this._ErrorMsg.TryGetValue(lang, out string msg))
                                {
                                        msg = RpcClient.RpcClient.Config.GetConfigVal(string.Format("DefErrorMsg_{0}", lang));
                                }
                                return new ErrorMsg
                                {
                                        Lang = lang,
                                        ErrorCode = this.ErrorCode,
                                        ErrorId = this._ErrorId,
                                        Msg = msg
                                };
                        }
                }
        }
}
