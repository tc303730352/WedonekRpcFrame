using System;
using System.IO;
using System.IO.Compression;
using WeDonekRpc.HttpService.Config;
using WeDonekRpc.Helper;

namespace WeDonekRpc.HttpService.Collect
{
    internal class GzipCollect
    {
        private static GzipConfig _Config;
        private static DataQueueHelper<string> _CacheQuery = null;
        private static string _CacheDir;

        public static void Enable(GzipConfig config)
        {
            if (_CacheQuery == null)
            {
                _CacheQuery = new DataQueueHelper<string>(_WriteCache, 1000, 1000);
            }
            _CacheDir = config.CacheDir;
            _Config = config;
        }
        public static void Stop(GzipConfig config)
        {
            if (_CacheQuery != null)
            {
                _CacheQuery.Dispose();
                _CacheQuery = null;
            }
            _Config = config;
        }

        public static bool CheckTextIsZip(int len)
        {
            return _Config.IsZipApiText && len >= _Config.TextLength;
        }
        public static bool CheckIsZip(FileInfo file, out FileInfo cacheFile)
        {
            if (_Config.IsEnable && _Config.Extensions.IsExists(file.Extension) && file.Length >= _Config.MinFileSzie)
            {
                cacheFile = _GetCacheFile(file);
                return true;
            }
            cacheFile = null;
            return false;
        }
        private static string _GetCacheFilePath(FileInfo file)
        {
            string type = file.Extension.Remove(0, 1).ToLower();
            string name = string.Join("_", file.Length, file.LastWriteTime.ToLong(), file.FullName).GetMd5();
            return string.Format(@"{0}\{1}\{2}.gzip", _CacheDir, type, name);
        }
        private static FileInfo _GetCacheFile(FileInfo file)
        {
            if (_Config.IsEnable)
            {
                string path = _GetCacheFilePath(file);
                if (File.Exists(path))
                {
                    return new FileInfo(path);
                }
                return null;
            }
            return null;
        }

        internal static void CacheFile(FileInfo file)
        {
            if (_Config.IsEnable)
            {
                _CacheQuery.AddOnlyQueue(file.FullName);
            }
        }

        public static bool CheckIsZip(Stream stream, string fileType)
        {
            return _Config.IsEnable && _Config.Extensions.IsExists(fileType) && stream.Length >= _Config.MinFileSzie;
        }

        private static void _WriteCache(string path)
        {
            FileInfo file = new FileInfo(path);
            if (!file.Exists)
            {
                return;
            }
            string cacheFile = _GetCacheFilePath(file);
            if (File.Exists(cacheFile))
            {
                return;
            }
            string tmpPath = cacheFile + ".tmp";
            FileInfo cache = new FileInfo(tmpPath);
            if (cache.Exists)
            {
                if (cache.LastWriteTime.AddMinutes(1) > DateTime.Now)
                {
                    return;
                }
                cache.Delete();
            }
            _Decompression(file, cache);
            cache.Refresh();
            if (!File.Exists(cacheFile) && cache.Exists)
            {
                cache.MoveTo(cacheFile);
            }
        }
        private static void _Decompression(FileInfo input, FileInfo output)
        {
            if (!output.Directory.Exists)
            {
                output.Directory.Create();
            }
            using (FileStream outStream = output.Open(FileMode.Create, FileAccess.ReadWrite, FileShare.Delete))
            {
                using (FileStream inStream = input.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    inStream.Position = 0;
                    using (GZipStream gzip = new GZipStream(outStream, CompressionMode.Compress, true))//创建压缩对象
                    {
                        inStream.CopyTo(gzip);
                        gzip.Flush();
                        gzip.Close();
                    }
                }
                outStream.Flush();
                outStream.Close();
            }
        }

    }
}
