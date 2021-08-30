using RpcHelper.Config;

namespace HttpService.Config
{
        internal class UpConfig
        {
                private static readonly long _LimitMaxFileSize = 10485760;//文件不能大于10MB

                static UpConfig()
                {
                        long size = LocalConfig.Local.GetValue<long>("http:LimitMaxFileSize", 0);
                        if (size != 0)
                        {
                                _LimitMaxFileSize = size * 1024;//单位KB 转byte
                        }

                }
                /// <summary>
                /// 限制最大文件上传大小
                /// </summary>
                public static long LimitMaxFileSize => _LimitMaxFileSize;
        }
}
