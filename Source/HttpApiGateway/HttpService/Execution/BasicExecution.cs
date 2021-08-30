using System;
using System.Net;

using HttpService.Collect;
using HttpService.Interface;

namespace HttpService.Execution
{
        internal class BasicExecution : IBasicExecution
        {
                public void Execution(IHttpServer server, HttpListenerContext context)
                {
                        if (RouteCollect.GetHandler(server, context, out IBasicHandler handler))
                        {
                                handler.Init(context);
                                try
                                {
                                        if (handler.Verification())
                                        {
                                                handler.Execute();
                                        }
                                }
                                catch (Exception e)
                                {
                                        HttpLog.AddErrorLog(server, e);
                                        handler.Response.SetHttpStatus(HttpStatusCode.InternalServerError);
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
