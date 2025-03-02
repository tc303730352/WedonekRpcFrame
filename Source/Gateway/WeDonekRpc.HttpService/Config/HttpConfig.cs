using System;
using System.Net;
using System.Text;
using WeDonekRpc.HttpService.Interface;
using WeDonekRpc.Helper.Config;

namespace WeDonekRpc.HttpService.Config
{
    internal class HttpConfig : IHttpConfig
    {
        private static readonly ICrossConfig _Cross = new CrossConfig();
        public HttpConfig ()
        {
            this.Up = new UpConfig();
            this.File = new DefHttpFileConfig(_Cross);
            this.Gzip = new GzipConfig();
            this.Log = new LogConfig();
            this._Load();
        }
        private void _Load ()
        {
            IConfigSection section = LocalConfig.Local.GetSection("gateway:http:server");
            this.Realm = section.GetValue("Realm");
            this.AuthenticationSchemes = section.GetValue("AuthenticationSchemes", AuthenticationSchemes.Anonymous);
            this.TimeOut = section.GetValue<TimeOutConfig>("Timeout", new TimeOutConfig());
            this.RealRequestUri = section.GetValue<Uri>("RealRequestUri");
            this.IgnoreWriteExceptions = section.GetValue("IgnoreWriteExceptions", true);
            this.CertHashVal = section.GetValue("CertHashVal");
            string encoding = section.GetValue("ResponseEncoding", "utf-8");
            this.ResponseEncoding = Encoding.GetEncoding(encoding);
            encoding = section.GetValue("RequestEncoding", "utf-8");
            this.RequestEncoding = Encoding.GetEncoding(encoding);
        }
        /// <summary>
        /// 跨域配置
        /// </summary>
        public ICrossConfig Cross => _Cross;


        public HttpFileConfig File { get; }

        public GzipConfig Gzip { get; }

        public UpConfig Up { get; }

        /// <summary>
        /// 证书的Hash值
        /// </summary>
        public string CertHashVal { get; private set; }
        /// <summary>
        /// 日志配置
        /// </summary>
        public LogConfig Log { get; }

        /// <summary>
        /// 请求编码
        /// </summary>
        public Encoding RequestEncoding { get; private set; }
        /// <summary>
        /// 响应编码
        /// </summary>
        public Encoding ResponseEncoding { get; private set; }
        /// <summary>
        /// 实际请求地址
        /// </summary>
        public Uri RealRequestUri { get; private set; }
        /// <summary>
        /// 忽略写入异常
        /// </summary>
        public bool IgnoreWriteExceptions { get; private set; }
        /// <summary>
        /// 超时配置
        /// </summary>
        public TimeOutConfig TimeOut { get; private set; }
        /// <summary>
        /// 领域
        /// </summary>
        public string Realm { get; private set; }
        /// <summary>
        /// 认证方案
        /// </summary>
        public AuthenticationSchemes AuthenticationSchemes { get; private set; }


        public void AddFileDir (HttpFileConfig config)
        {
            config.Init(_Cross);
        }
    }
}
