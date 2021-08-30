using System;
using System.Net;

using HttpService.Collect;
using HttpService.Interface;
using HttpService.Log;

namespace HttpService.Execution
{
        internal class LogExecution : IBasicExecution
        {
                public void Execution(IHttpServer server, HttpListenerContext context)
                {
                        HttpLogHelper log = new HttpLogHelper(context);
                        log.IntiLog();
                        if (RouteCollect.GetHandler(server, context, out IBasicHandler handler))
                        {
                                handler.Init(context);
                                try
                                {
                                        if (handler.Verification())
                                        {
                                                log.Init(handler);
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
                                        log.SaveLog();
                                        handler.Dispose();
                                }
                        }
                        else
                        {
                                context.Response.KeepAlive = false;
                                context.Response.StatusCode = 404;
                                context.Response.Close();
                                log.SaveLog();
                        }

                }
        }
}
