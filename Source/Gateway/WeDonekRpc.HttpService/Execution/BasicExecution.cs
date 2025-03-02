using WeDonekRpc.HttpService.Interface;
using System;
using System.Net;

namespace WeDonekRpc.HttpService.Execution
{
    internal class BasicExecution : IBasicExecution
    {
        public void Execution(IHttpServer server, HttpListenerContext context)
        {
            IBasicHandler handler = HttpRouteService.GetHandler(server, context);
            if (handler != null)
            {
                try
                {
                    if (handler.Verification())
                    {
                        handler.Execute();
                    }
                    else if(!handler.Response.IsEnd)
                    {
                        handler.Response.SetHttpStatus(HttpStatusCode.Unauthorized);
                    }
                }
                catch (Exception e)
                {
                    handler.ExecError(e);
                }
                finally
                {
                    handler.Dispose();
                }
            }
            else
            {
                context.Response.KeepAlive = false;
                context.Response.StatusCode = 404;
                context.Response.Close();
            }
        }
    }
}
