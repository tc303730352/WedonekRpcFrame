using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Service.Interface
{
        public interface IErrorLangMsgCollect
        {
                ErrorLang[] GetErrorMsg(long[] errorId);
                void SyncError(long errorId, string lang, string msg);
                string GetErrorMsg(long errorId, string lang);
        }
}