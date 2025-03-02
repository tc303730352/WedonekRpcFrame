using WeDonekRpc.Helper;
using RpcSync.Collect;
using WeDonekRpc.Helper.Error;

namespace RpcSync.Service.Error
{
    internal class ErrorEvent : IErrorEvent
    {
        private readonly IErrorCollect _Error;
        public ErrorEvent (IErrorCollect error)
        {
            this._Error = error;
        }
        public void DropError (ErrorMsg error)
        {
        }

        public ErrorMsg GetError (string code, string lang)
        {
            return this._Error.FindError(code, lang);
        }

        public string FindErrorCode (long errorId)
        {
            string code = this._Error.FindErrorCode(errorId);
            if (code.IsNull())
            {
                return "public.unknown";
            }
            return code;
        }

        public long FindErrorId (string code)
        {
            return this._Error.GetOrAdd(code);
        }

        public void DropError (long errorId, string code)
        {

        }
    }
}
