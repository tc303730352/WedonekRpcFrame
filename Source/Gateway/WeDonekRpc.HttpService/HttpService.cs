using System.Collections.Generic;
using WeDonekRpc.HttpService.Collect;
using WeDonekRpc.HttpService.Config;
using WeDonekRpc.HttpService.Helper;
using WeDonekRpc.HttpService.Interface;
namespace WeDonekRpc.HttpService
{
    public class HttpService
    {
        private static readonly Dictionary<string, IHttpServer> _ServerList = [];

        public static IHttpConfig Config { get; }

        static HttpService ()
        {
            Config = new HttpConfig();
        }
        public static void RegService (string uri)
        {
            IHttpServer server = new Server.Server(uri, Config.CertHashVal, Config);
            _ServerList.Add(uri, server);
            server.Start();
        }
        public static void AddFileDir (FileDirConfig dir)
        {
            dir.InitConfig(Config.Cross);
        }
        public static void AddRoute (IBasicHandler handler)
        {
            RouteCollect.AddRoute(handler);
        }
        public static void RemoveRoute (string path, bool isRegex)
        {
            RouteCollect.RemoveRoute(path, isRegex);
        }
        public static void ReplaceRoute (IBasicHandler old, IBasicHandler route)
        {
            RouteCollect.ReplaceRoute(old, route);
        }
        public static void StopService ()
        {
            if (_ServerList.Count > 0)
            {
                foreach (IHttpServer i in _ServerList.Values)
                {
                    i.Close();
                }
            }
        }

        public static string GetFileSavePath (string path)
        {
            return FileHelper.GetFileFullPath(path);
        }
    }
}
