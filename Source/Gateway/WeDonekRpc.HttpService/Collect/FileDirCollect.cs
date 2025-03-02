using System;
using System.Collections.Concurrent;
using System.Linq;
using WeDonekRpc.HttpService.Config;
using WeDonekRpc.HttpService.Handler;
using WeDonekRpc.HttpService.Interface;
using WeDonekRpc.HttpService.Model;
using WeDonekRpc.Helper;

namespace WeDonekRpc.HttpService.Collect
{
    internal class FileDirCollect
    {
        private static string[] _VirtualPath = null;
        private static readonly ConcurrentDictionary<string, string> _FileDir = new ConcurrentDictionary<string, string>();
        public static string FindDirPath (string path)
        {
            path = path.Replace('\\', '/');
            string dir = _VirtualPath.Find(c => path.StartsWith(c));
            if (dir.IsNull())
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
            return _FileDir[dir];
        }

        public static void AddDir (FileDirConfig config, ICrossConfig cross)
        {
            if (_FileDir.ContainsKey(config.VirtualPath))
            {
                return;
            }
            DirConfig dir = new DirConfig
            {
                defCacheSet = config.DefCacheSet,
                dirPath = config.DirPath,
                cross = cross,
                cacheSet = config.Caches == null ? [] : config.Caches,
                virtualLen = config.VirtualPath.Length,
                virtualPath = config.VirtualPath.ToLower(),
            };
            if (_FileDir.TryAdd(config.VirtualPath, dir.dirPath))
            {
                string path = config.VirtualPath;
                string regex = config.GetRegex();
                int sort = 10;
                if (path != "/")
                {
                    sort += path.Split('/').Length * 5;
                }
                IBasicHandler handler = new FileHandler(regex, dir, (short)sort);
                RouteCollect.AddRoute(handler);
                _VirtualPath = _FileDir.Keys.OrderByDescending(a => a.Length).ToArray();
            }
        }
    }
}
