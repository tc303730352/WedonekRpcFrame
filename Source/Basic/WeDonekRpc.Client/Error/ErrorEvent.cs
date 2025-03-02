using WeDonekRpc.Client.Controller;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Resource;
using WeDonekRpc.Helper.Error;

namespace WeDonekRpc.Client.Error
{
    internal class ErrorEvent : IErrorEvent
    {
        private IErrorService _Service;
        public ErrorEvent(IErrorService service)
        {
            _Service = service;
        }
        public IErrorManage Error { get; set; }


        public void DropError(ErrorMsg error)
        {
        }

        public void DropError(long errorId, string code)
        {
            _Service.Drop(code);
        }

        public ErrorMsg GetError(string code, string lang)
        {
            if (!this._Service.GetError(code, out ErrorController obj))
            {
                return null;
            }
            string text= obj.GetErrorText(lang);
            return new ErrorMsg
            {
                ErrorId = obj.ErrorId,
                Lang = lang,
                ErrorCode = code,
                Text = text
            };
        }

        public string FindErrorCode(long errorId)
        {
            return ErrorRemote.FindError(errorId);
        }

        public long FindErrorId(string code)
        {
            if (!this._Service.GetError(code, out ErrorController obj))
            {
                return 0;
            }
            return obj.ErrorId;
        }
    }
}
