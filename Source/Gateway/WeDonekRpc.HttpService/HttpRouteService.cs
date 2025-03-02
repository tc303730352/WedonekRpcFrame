using System.Net;
using WeDonekRpc.HttpService.Collect;
using WeDonekRpc.HttpService.Interface;
using WeDonekRpc.HttpService.Rewrite;

namespace WeDonekRpc.HttpService
{
    internal class HttpRouteService
    {
        internal static IBasicHandler GetHandler (IHttpServer server, HttpListenerContext context)
        {
            if (context.Request.Url == null)
            {
                return null;
            }
            string path = context.Request.Url.AbsolutePath.ToLower();
            if (UrlRewriteCollect.GetRoute(path, context.Request.Url, out RouteParam param))
            {
                IBasicHandler handler = RouteCollect.GetHandler(param.Action);
                handler?.Init(context, param.Args);
                return handler;
            }
            else
            {
                IBasicHandler handler = RouteCollect.GetHandler(path);
                handler?.Init(context, null);
                return handler;
            }
        }
    }
}
