using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using HttpService.Collect;

using RpcHelper.Config;

namespace HttpService.Config
{
        /// <summary>
        /// 服务配置
        /// </summary>
        public class ServerConfig
        {
                private static bool _IsEnableLog = LocalConfig.Local.GetValue("http:IsEnableLog", false);
                public static readonly string FileSavePath = null;

                private static readonly string _CertHashVal = LocalConfig.Local["http:CertHashVal"];

                private static readonly string[] _LimitUrlReferrer = null;

                private static readonly bool _IsLimit = false;

                /// <summary>
                /// 响应编码
                /// </summary>
                public static Encoding ResponseEncoding = Encoding.UTF8;

                static ServerConfig()
                {
                        _LimitUrlReferrer = LocalConfig.Local.GetValue("http:LimitUrlReferrer", new string[0]);
                        if (_LimitUrlReferrer.Length != 0)
                        {
                                _IsLimit = true;
                        }
                        string path = LocalConfig.Local.GetValue("http:httpFilePath", AppDomain.CurrentDomain.BaseDirectory);
                        if (path.StartsWith(@"\"))
                        {
                                path = path.Remove(0, 1);
                        }
                        if (!Regex.IsMatch(path, @"^[a-z]{1}[:][\\].*$", RegexOptions.IgnoreCase))
                        {
                                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
                        }
                        if (path.EndsWith(@"\"))
                        {
                                path = path.Remove(path.Length - 1, 1);
                        }
                        FileSavePath = path;
                }

                /// <summary>
                /// 证书的Hash值
                /// </summary>
                internal static string CertHashVal => _CertHashVal;
                /// <summary>
                /// 是否启用日志
                /// </summary>
                public static bool IsEnableLog
                {
                        get => _IsEnableLog;
                        set
                        {
                                if (value != _IsEnableLog)
                                {
                                        _IsEnableLog = value;
                                        ServiceCollect.InitHttpExecution(value);
                                }
                        }
                }
        }
}
