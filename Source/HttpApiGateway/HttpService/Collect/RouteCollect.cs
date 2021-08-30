using System.Linq;
using System.Net;

using HttpService.FileExtend;
using HttpService.Handler;
using HttpService.Interface;

using RpcHelper;
namespace HttpService.Collect
{
        internal class RouteCollect
        {
                private static IBasicHandler[] _RouteList = new IBasicHandler[]
                {
                        new ImgFileHandler(),
                        new CssFileHandler(),
                        new JsFileHandler(),
                        new FontFileHandler(),
                        new HtmlFileHandler(),
                        new TxtFileHandler(),
                        new JsonFileHandler(),
                };

                public static IBasicHandler[] RouteList => _RouteList;

                internal static void Sort()
                {
                        _RouteList = _RouteList.OrderByDescending(a => a.SortNum).ToArray();
                }


                internal static bool GetHandler(IHttpServer server, HttpListenerContext context, out IBasicHandler handler)
                {
                        if (context.Request.Url == null || context.Request.Url.Authority != server.Uri.Authority)
                        {
                                handler = null;
                                return false;
                        }
                        string path = context.Request.Url.AbsolutePath.ToLower();
                        IBasicHandler temp = _RouteList.Find(a => a.IsUsable(path));
                        if (temp != null)
                        {
                                handler = (IBasicHandler)temp.Clone();
                                return true;
                        }
                        handler = null;
                        return false;
                }

                public static void AddRoute(BasicHandler handler)
                {
                        _RouteList = _RouteList.Add(handler);
                }
        }
}
