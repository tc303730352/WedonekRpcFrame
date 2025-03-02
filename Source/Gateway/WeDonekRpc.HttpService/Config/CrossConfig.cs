using WeDonekRpc.HttpService.Interface;
using WeDonekRpc.Helper.Config;

namespace WeDonekRpc.HttpService.Config
{
    public class CrossConfig : ICrossConfig
    {
        private static DefCrossConfig _Config;
        static CrossConfig ()
        {
            _Config = new DefCrossConfig();
            IConfigSection section = LocalConfig.Local.GetSection("gateway:cross");
            section.AddRefreshEvent(_Refresh);
        }

        private static void _Refresh (IConfigSection section, string name)
        {
            _Config = new DefCrossConfig
            {
                IsEnable = section.GetValue<bool>("IsEnable", true),
                AllowCredentials = section.GetValue<bool>("AllowCredentials", true),
                AllowUrlReferrer = section.GetValue<string[]>("AllowUrlReferrer"),
                AllowHead = section.GetValue<string>("AllowHead", "*"),
                MaxAge = section.GetValue<string>("MaxAge", "3600"),
                Method = section.GetValue<string>("Method", "POST,GET,OPTIONS")
            };
        }

        public bool IsEnable => _Config.IsEnable;
        /// <summary>
        /// 允许跨域
        /// </summary>
        public bool AllowCredentials => _Config.AllowCredentials;

        /// <summary>
        /// 跨域限定允许访问来源
        /// </summary>
        public string[] AllowUrlReferrer => _Config.AllowUrlReferrer;
        /// <summary>
        /// 跨域限定头部
        /// </summary>
        public string AllowHead => _Config.AllowHead;
        /// <summary>
        /// 有效时间
        /// </summary>
        public string MaxAge => _Config.MaxAge;

        /// <summary>
        /// 允许的请求方式 ,号分隔
        /// </summary>
        public string Method => _Config.Method;

    }
}
