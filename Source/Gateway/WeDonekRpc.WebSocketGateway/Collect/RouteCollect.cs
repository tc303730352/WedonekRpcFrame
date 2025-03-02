using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpWebSocket.Interface;
using WeDonekRpc.WebSocketGateway.Helper;
using WeDonekRpc.WebSocketGateway.Interface;

namespace WeDonekRpc.WebSocketGateway.Collect
{
    [Client.Attr.ClassLifetimeAttr(Client.Attr.ClassLifetimeType.SingleInstance)]
    internal class RouteCollect : IRouteCollect
    {
        private static readonly ConcurrentDictionary<string, ApiHandler> _RouteApi = new ConcurrentDictionary<string, ApiHandler>();

        private static int _Id = 0;

        internal static string CreateApiId ()
        {
            return Interlocked.Add(ref _Id, 1).ToString();
        }
        #region 注册路由

        public static async void Adds (ApiHandler[] routes, Action<ApiHandler> action)
        {
            await Task.Factory.StartNew(() =>
            {
                routes.ForEach(a =>
                {
                    Add(a);
                    action(a);
                });
            });
        }
        public static void Add (ApiHandler route)
        {

            if (_RouteApi.TryAdd(route.LocalPath, route))
            {
                route.Init();
            }
            else
            {
                throw new ErrorException("gateway.route.reg.fail");
            }
        }
        #endregion

        public static void ExecRoute (IApiService service)
        {
            IUserPage page = ApiHelper.GetPage(service.Request.Content, out byte[] content);
            if (page == null || page.Direct.IsNull())
            {
                throw new ErrorException("http.web.page.analysis.error");
            }
            else if (!_RouteApi.TryGetValue(page.Direct, out ApiHandler handler) || handler.IsEnable == false)
            {
                throw new ErrorException("http.404");
            }
            else if (!ApiPlugInService.Exec(service, handler, out string error))
            {
                throw new ErrorException(error);
            }
            else
            {
                using (handler)
                {
                    handler.Execute(service, page, content);
                }
            }
        }

        public void EnableByPath (string path)
        {
            if (_RouteApi.TryGetValue(path, out ApiHandler route))
            {
                route.Enable();
            }
        }

        public void DisableByPath (string path)
        {
            if (_RouteApi.TryGetValue(path, out ApiHandler route))
            {
                route.Disable();
            }
        }

        public void RemoveByPath (string path)
        {
            if (_RouteApi.TryRemove(path, out ApiHandler route))
            {
                route.Dispose();
            }
        }
    }
}
