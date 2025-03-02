using System;
using System.IO;
using WeDonekRpc.HttpService.Collect;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Config;

namespace WeDonekRpc.HttpService.Config
{
    public class GzipConfig
    {
        private static readonly string[] _DefExtensions = new string[] { ".txt", ".html", ".css", ".js", ".htm", ".xml", ".json", ".docx", ".doc", ".xlsx", ".xls" };
        public GzipConfig ()
        {
            IConfigSection section = LocalConfig.Local.GetSection("gateway:http:gzip");
            section.AddRefreshEvent(this._Refresh);
        }
        private bool _IsInit = false;
        private void _Refresh (IConfigSection section, string name)
        {
            if (this._IsInit)
            {
                return;
            }
            this._IsInit = true;
            this.IsEnable = section.GetValue("IsEnable", true);
            this.Extensions = section.GetValue<string[]>("Extensions", _DefExtensions);
            this.MinFileSzie = section.GetValue("MinFileSzie", 10240);
            this.IsCacheFile = section.GetValue("IsCacheFile", true);
            this.CacheDir = section.GetValue("CacheDir", "ZipCache");
            this.IsZipApiText = section.GetValue("IsZipApiText", true);
            this.TextLength = section.GetValue("TextLength", 1000);
            this._Init();
        }
        /// <summary>
        /// 是否启用压缩
        /// </summary>
        public bool IsEnable
        {
            get;
            private set;
        }
        /// <summary>
        /// 需要压缩的文件扩展名
        /// </summary>
        public string[] Extensions
        {
            get;
            private set;
        }
        /// <summary>
        /// 最小文件大小
        /// </summary>
        public int MinFileSzie
        {
            get;
            private set;
        }
        /// <summary>
        /// 是否缓存为文件
        /// </summary>
        public bool IsCacheFile
        {
            get;
            private set;
        }
        /// <summary>
        /// 缓存目录
        /// </summary>
        public string CacheDir
        {
            get;
            private set;
        }
        /// <summary>
        /// 是否压缩API接口返回的文本
        /// </summary>
        public bool IsZipApiText
        {
            get;
            private set;
        }
        /// <summary>
        /// API接口返回的文本限定长度
        /// </summary>
        public int TextLength
        {
            get;
            private set;
        }

        private void _Init ()
        {
            if (this.IsEnable)
            {
                if (this.CacheDir.IsNull())
                {
                    this.CacheDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ZipCache");
                }
                else if (!Path.IsPathRooted(this.CacheDir))
                {
                    this.CacheDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this.CacheDir);
                }
                if (this.CacheDir.EndsWith('\\'))
                {
                    this.CacheDir = this.CacheDir.Remove(this.CacheDir.Length - 1, 1);
                }
                GzipCollect.Enable(this);
            }
            else
            {
                GzipCollect.Stop(this);
            }
        }
    }
}
