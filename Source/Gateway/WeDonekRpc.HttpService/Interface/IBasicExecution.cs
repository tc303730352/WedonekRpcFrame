using System.Net;

namespace WeDonekRpc.HttpService.Interface
{
    internal interface IBasicExecution
    {
        void Execution (IHttpServer server, HttpListenerContext context);
    }
}