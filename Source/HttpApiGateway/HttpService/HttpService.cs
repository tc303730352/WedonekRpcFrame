using System;
using System.Collections.Generic;

using HttpService.Collect;
using HttpService.Handler;
using HttpService.Interface;
namespace HttpService
{
        public class HttpService
        {
                private static readonly Dictionary<Uri, IHttpServer> _ServerList = new Dictionary<Uri, IHttpServer>();
                public static void RegService(Uri uri)
                {
                        IHttpServer server = new Server.Server(uri);
                        _ServerList.Add(uri, server);
                        RouteCollect.Sort();
                        server.Start();
                }
                public static void AddRoute(BasicHandler handler)
                {
                        RouteCollect.AddRoute(handler);
                }
                public static void StopService()
                {
                        if (_ServerList.Count > 0)
                        {
                                foreach (IHttpServer i in _ServerList.Values)
                                {
                                        i.Close();
                                }
                        }
                }
                public static void Sort()
                {
                        RouteCollect.Sort();
                }
        }
}
