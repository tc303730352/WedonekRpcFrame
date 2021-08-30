using System;
using System.IO;
using System.Text.RegularExpressions;

using RpcHelper.Config;
namespace HttpService.Config
{
        internal class FileConfig
        {
                private static readonly string _FileSavePath = LocalConfig.Local["httpFilePath"];
                static FileConfig()
                {
                        if (string.IsNullOrEmpty(_FileSavePath))
                        {
                                _FileSavePath = AppDomain.CurrentDomain.BaseDirectory;
                        }
                        if (_FileSavePath.StartsWith(@"\"))
                        {
                                _FileSavePath = _FileSavePath.Remove(0, 1);
                        }
                        if (!Regex.IsMatch(_FileSavePath, @"^[a-z]{1}[:][\\].*$", RegexOptions.IgnoreCase))
                        {
                                _FileSavePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _FileSavePath);
                        }
                        if (_FileSavePath.EndsWith(@"\"))
                        {
                                _FileSavePath = _FileSavePath.Remove(_FileSavePath.Length - 1, 1);
                        }
                }
                /// <summary>
                ///通过URI 获取文件路径
                /// </summary>
                /// <param name="uri"></param>
                /// <returns></returns>
                public static string GetFilePath(Uri uri)
                {
                        return string.Format("{0}{1}", _FileSavePath, uri.AbsolutePath.Replace("/", "\\"));
                }
                public static string GetFileUri(Uri uri, string path)
                {
                        path = GetFilePath(path);
                        if (!path.StartsWith(_FileSavePath))
                        {
                                return null;
                        }
                        path = path.Replace(_FileSavePath, "").Replace(@"\", "/");
                        return string.Format("{0}://{1}{2}", uri.Scheme, uri.Authority, path);
                }
                public static string GetFilePath(string path)
                {
                        path = path.Replace("/", @"\");
                        if (path.StartsWith(@"\"))
                        {
                                return string.Format("{0}{1}", _FileSavePath, path);
                        }
                        else if (!Regex.IsMatch(path, @"^[a-z]{1}[:][\\].*$", RegexOptions.IgnoreCase))
                        {
                                return Path.Combine(_FileSavePath, path);
                        }
                        return path;
                }
        }
}
