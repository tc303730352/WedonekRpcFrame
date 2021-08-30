using System;
using System.IO;
using System.Text.RegularExpressions;

using HttpService.Config;

namespace HttpService.Helper
{
        public class FileHelper
        {
                /// <summary>
                ///通过URI 获取文件路径
                /// </summary>
                /// <param name="uri"></param>
                /// <returns></returns>
                public static string GetFilePath(Uri uri)
                {
                        return string.Format("{0}{1}", ServerConfig.FileSavePath, uri.AbsolutePath.Replace("/", "\\"));
                }
                public static string GetFilePath(string path)
                {
                        path = path.Replace("/", @"\");
                        if (path.StartsWith(@"\"))
                        {
                                return string.Format("{0}{1}", ServerConfig.FileSavePath, path);
                        }
                        else if (!Regex.IsMatch(path, @"^[a-z]{1}[:][\\].*$", RegexOptions.IgnoreCase))
                        {
                                return Path.Combine(ServerConfig.FileSavePath, path);
                        }
                        return path;
                }
                public static string GetFileSavePath(string path)
                {
                        path = path.Replace("/", @"\");
                        if (path.StartsWith(@"\"))
                        {
                                return string.Format("{0}{1}", ServerConfig.FileSavePath, path);
                        }
                        else if (!Regex.IsMatch(path, @"^[a-z]{1}[:][\\].*$", RegexOptions.IgnoreCase))
                        {
                                return Path.Combine(ServerConfig.FileSavePath, path);
                        }
                        return path;
                }

                public static string GetFileUri(Uri uri, string savePath)
                {
                        string path = GetFilePath(savePath);
                        if (!savePath.StartsWith(ServerConfig.FileSavePath))
                        {
                                return null;
                        }
                        path = path.Replace(ServerConfig.FileSavePath, "").Replace(@"\", "/");
                        return string.Format("{0}://{1}{2}", uri.Scheme, uri.Authority, path);
                }
        }
}
