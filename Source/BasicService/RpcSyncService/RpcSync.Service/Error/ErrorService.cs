using RpcSync.Collect;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper.Error;
using WeDonekRpc.Model.ErrorManage;

namespace RpcSync.Service.Error
{
    internal class ErrorService : Interface.IErrorService
    {
        private readonly IRemoteSendService _Send;
        private readonly IErrorCollect _Error;
        public ErrorService ( IRemoteSendService send, IErrorCollect error )
        {
            this._Send = send;
            this._Error = error;
        }
        public void InitError ( long errorId )
        {
            LocalErrorManage.Drop(errorId);
            this._Error.Refresh(errorId);
            this._Send.BroadcastMsg(new RefreshError
            {
                ErrorId = errorId
            });
        }
    }
}
