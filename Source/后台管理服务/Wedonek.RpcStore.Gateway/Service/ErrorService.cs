using HttpApiGateway.Model;

using RpcClient;

using RpcHelper;

using Wedonek.RpcStore.Gateway.Interface;
using Wedonek.RpcStore.Gateway.Model;
using Wedonek.RpcStore.Service.Interface;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Service
{
        internal class ErrorService : IErrorService
        {
                private readonly IErrorCollect _Error = null;

                private readonly IErrorLangMsgCollect _ErrorLang = null;

                public ErrorService(IErrorLangMsgCollect lang, IErrorCollect error)
                {
                        this._Error = error;
                        this._ErrorLang = lang;
                }
                public ErrorData[] QueryError(PagingParam<ErrorParam> query, out long count)
                {
                        ErrorDatum[] errors = this._Error.QueryError(query.Param.ErrorCode, query.ToBasicPaging(), out count);
                        if (errors == null)
                        {
                                return new ErrorData[0];
                        }
                        long[] ids = errors.Convert(a => a.IsPerfect, a => a.Id);
                        if (ids.Length == 0)
                        {
                                return errors.ConvertMap<ErrorDatum, ErrorData>();
                        }
                        ErrorLang[] langs = this._ErrorLang.GetErrorMsg(ids);
                        return errors.ConvertMap<ErrorDatum, ErrorData>((a, b) =>
                        {
                                b.Zh = langs.Find(c => c.ErrorId == a.Id && c.Lang == "zh", c => c.Msg);
                                b.En = langs.Find(c => c.ErrorId == a.Id && c.Lang == "en", c => c.Msg);
                                return b;
                        });
                }
                public long AddError(ErrorAddDatum add)
                {
                        return this._Error.SyncError(add);
                }
                public void SetErrorMsg(SetErrorParam param)
                {
                        this._Error.SetErrorMsg(param.ErrorId, param.Lang, param.Msg);
                }
                public ErrorAddDatum GetError(long id, string lang)
                {
                        ErrorDatum error = this._Error.GetError(id);
                        if (error == null)
                        {
                                throw new ErrorException("rpc.error.not.find");
                        }
                        return new ErrorAddDatum
                        {
                                Msg = this._ErrorLang.GetErrorMsg(id, lang),
                                ErrorCode = error.ErrorCode,
                                Lang = lang
                        };
                }
        }
}
