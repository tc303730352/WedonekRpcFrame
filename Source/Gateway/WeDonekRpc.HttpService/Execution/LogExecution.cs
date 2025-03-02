using System;
using System.Net;
using WeDonekRpc.HttpService.Config;
using WeDonekRpc.HttpService.Interface;
using WeDonekRpc.HttpService.Log;

namespace WeDonekRpc.HttpService.Execution
{
    internal class LogExecution : IBasicExecution
    {
        private readonly LogConfig _Config;
        public LogExecution ( LogConfig config )
        {
            this._Config = config;
        }

        public void Execution ( IHttpServer server, HttpListenerContext context )
        {
            HttpLogHelper log = new HttpLogHelper(context, this._Config);
            log.IntiLog();
            IBasicHandler handler = HttpRouteService.GetHandler(server, context);
            if ( handler != null )
            {
                try
                {
                    if ( handler.Verification() )
                    {
                        log.Init(handler);
                        handler.Execute();
                    }
                }
                catch ( Exception e )
                {
                    handler.ExecError(e);
                }
                finally
                {
                    log.SaveLog();
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
