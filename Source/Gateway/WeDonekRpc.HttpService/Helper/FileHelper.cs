using System;
using System.IO;
using System.Text;
using WeDonekRpc.HttpService.Collect;
using WeDonekRpc.HttpService.Config;
using WeDonekRpc.HttpService.Model;
using WeDonekRpc.Helper;

namespace WeDonekRpc.HttpService.Helper
{
    internal class FileHelper
    {
        private static readonly int _MinFileSize = 10485760;
        private static readonly HttpFileConfig _Config = HttpService.Config.File;
        /// <summary>
        ///通过URI 获取文件路径
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string GetFilePath(Uri uri, DirConfig config)
        {
            return string.Format("{0}{1}", config.dirPath, _GetPath(uri, config));
        }
        private static string _GetPath(Uri uri, DirConfig config)
        {
            string path = uri.AbsolutePath;
            if (config.virtualPath != "/" && path.ToLower().StartsWith(config.virtualPath))
            {
                return path.Remove(0, config.virtualLen).Replace('/', '\\');
            }
            return path.Replace('/', '\\');
        }
        public static string FormatFilePath(string path, DirConfig config)
        {
            path = path.Replace('/', '\\');
            if (path[0] == '\\')
            {
                return string.Concat(config.dirPath, path);
            }
            else if (!Path.IsPathRooted(path))
            {
                return Path.Combine(config.dirPath, path);
            }
            return path;
        }
        public static string GetTempSavePath(string path)
        {
            path = path.Replace('/', '\\');
            if (path[0] == '\\')
            {
                return string.Concat(_Config.TempDirPath, path);
            }
            else if (!Path.IsPathRooted(path))
            {
                return Path.Combine(_Config.TempDirPath, path);
            }
            return path;
        }
        public static string GetFileFullPath(string path)
        {
            if (Path.IsPathRooted(path))
            {
                return path;
            }
            string dirPath = FileDirCollect.FindDirPath(path);
            return Path.Combine(dirPath, path);
        }
        public static string GetFileFullPath(Uri uri)
        {
            string dirPath = FileDirCollect.FindDirPath(uri.AbsolutePath);
            return Path.Combine(dirPath, uri.AbsolutePath.Replace('/','\\'));
        }
        public static string GetFileUri(Uri uri, string savePath, DirConfig config)
        {
            string path = FormatFilePath(savePath, config);
            if (!path.StartsWith(config.dirPath))
            {
                return null;
            }
            path = path.Replace(config.dirPath, string.Empty).Replace(@"\", "/");
            if (config.virtualPath == "/")
            {
                return string.Concat(uri.Scheme, "://", uri.Authority, path);
            }
            return string.Concat(uri.Scheme, "://", uri.Authority, config.virtualPath, path);
        }

        #region 操作响应文件流
        public static void WriteStream(Stream output, Stream stream, FilePage page)
        {
            page.pages.ForEach(a =>
            {
                stream.Position = a.Begin;
                output.Write(a.Head, 0, a.Head.Length);
                FileHelper.WriteStream(output, stream, a.Len);
            });
            output.Write(page.end, 0, page.end.Length);
        }

        public static FilePage GetFilePage(Stream stream, string type, string[] ranges)
        {
            string boundary = Guid.NewGuid().ToString("N").Substring(0, 17).ToLower();
            FilePage page = new FilePage
            {
                len = stream.Length,
                boundary = boundary,
                type = type
            };
            byte[] end = Encoding.UTF8.GetBytes(string.Concat("--", boundary, "--"));
            long sum = end.Length;
            page.pages = ranges.ConvertAll(a => _GetBranchPage(a, page, ref sum));
            page.Size = sum;
            page.end = end;
            return page;
        }
        private static BranchPage _GetBranchPage(string range, FilePage page, ref long sum)
        {
            long end = page.len;
            string[] i = range.Split('-');
            if (i[1] != string.Empty)
            {
                end = long.Parse(i[1]) + 1;
            }
            else
            {
                i[1] = (page.len - 1).ToString();
            }
            long begin = i[0] == string.Empty ? page.len - end : long.Parse(i[0]);
            long size = end - begin;
            byte[] head = Encoding.UTF8.GetBytes(string.Format("--{0}\r\nContent-Type:{1}\r\nContent-Range:bytes {2}-{3}/{4}\r\n", page.boundary, page.type, i[0], i[1], page.len));
            sum += size + head.Length;
            return new BranchPage
            {
                Head = head,
                Begin = begin,
                Len = size,
                End = end
            };
        }
        public static void WriteStream(Stream stream, Stream file, long total)
        {
            int size = _MinFileSize;
            int num = (int)Math.Ceiling((decimal)total / size);
            if (num == 1)
            {
                file.CopyTo(stream);
            }
            else
            {
                num -= 1;
                for (int i = 0; i <= num; i++)
                {
                    if (i == num)
                    {
                        size = (int)(total - (i * size));
                    }
                    file.CopyTo(stream, size);
                }
            }
        }
        #endregion
    }
}
