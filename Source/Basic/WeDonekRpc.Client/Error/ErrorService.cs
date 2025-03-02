using System.Collections.Concurrent;
using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Controller;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Error;
using WeDonekRpc.Model.ErrorManage;

namespace WeDonekRpc.Client.Error
{
    [Attr.ClassLifetimeAttr(Attr.ClassLifetimeType.SingleInstance)]
    internal class ErrorService : IErrorService
    {

        private static readonly ConcurrentDictionary<string, ErrorController> _ErrorList = new ConcurrentDictionary<string, ErrorController>();


    
        public void Reset(long errorId, string code)
        {
            InitError obj = new InitError
            {
                ErrorCode = code,
                ErrorId = errorId
            };
            if (!RemoteCollect.Send(obj, out string error))
            {
                throw new ErrorException(error);
            }
        }

        public bool GetError(string code, out ErrorController error)
        {
            if (!_ErrorList.TryGetValue(code, out error))
            {
                error = _ErrorList.GetOrAdd(code, new ErrorController(code));
            }
            if (!error.Init())
            {
                _ErrorList.TryRemove(code, out error);
                error.Dispose();
                return false;
            }
            return error.IsInit;
        }
        public void Drop(string code)
        {
            if (_ErrorList.TryRemove(code, out ErrorController error))
            {
                error.Dispose();
            }
        }

       
        public void Refresh(long errorId)
        {
            LocalErrorManage.Drop(errorId);
        }
     
    }
}
