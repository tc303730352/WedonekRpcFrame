using System.Collections.Specialized;
using WeDonekRpc.Helper;

namespace WeDonekRpc.WebSocketGateway.Interface
{
    public interface IWebSocketService : IApiSocketService, System.IDisposable
    {
        ICurrentModular Modular { get; }

        NameValueCollection Form { get; }
        bool IsError { get; }
        string ErrorCode { get; }
        string PostString { get; }
        string ResponseText { get; }
        void InitService (IApiModular modular);
        void Reply ();
        void Reply (object result);
        void ReplyError (string error);
        void ReplyError (ErrorException error);
    }
}