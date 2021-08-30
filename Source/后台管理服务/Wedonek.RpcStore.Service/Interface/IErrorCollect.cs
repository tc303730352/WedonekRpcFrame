using RpcModel;

using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Service.Interface
{
        public interface IErrorCollect
        {
                long SetErrorMsg(long errorId, string lang, string msg);
                ErrorDatum GetError(long id);
                ErrorDatum[] QueryError(string code, IBasicPage paging, out long count);
                long SyncError(ErrorAddDatum add);
        }
}