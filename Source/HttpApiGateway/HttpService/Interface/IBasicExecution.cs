using System.Net;

namespace HttpService.Interface
{
        internal interface IBasicExecution
        {
                void Execution(IHttpServer server, HttpListenerContext context);
        }
}