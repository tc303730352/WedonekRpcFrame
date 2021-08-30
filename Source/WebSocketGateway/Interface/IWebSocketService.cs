using System.Collections.Specialized;

using RpcHelper;

namespace WebSocketGateway.Interface
{
        internal interface IWebSocketService : IApiSocketService
        {
                ICurrentModular Modular { get; }

                NameValueCollection Form { get; }
                bool IsError { get; }
                string ErrorCode { get; }
                string PostString { get; }
                string ResponseText { get; }
                void InitService(IApiModular modular);
                void Reply();
                void Reply(object result);
                void Reply(object result, object count);
                void ReplyError(string error);
                void ReplyError(ErrorException error);
        }
}