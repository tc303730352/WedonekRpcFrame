using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WeDonekRpc.HttpService.Collect;
using WeDonekRpc.HttpService.Interface;
using WeDonekRpc.HttpService.Model;
using WeDonekRpc.Helper;

namespace WeDonekRpc.HttpService.Config
{
    public class FileDirConfig
    {
        /// <summary>
        /// 文件目录
        /// </summary>
        public string DirPath
        {
            get;
            set;
        }
        /// <summary>
        /// 虚拟路径
        /// </summary>
        public string VirtualPath
        {
            get;
            set;
        } = "/";

        /// <summary>
        /// 可访问的扩展名
        /// </summary>
        public string[] Extension
        {
            get;
            set;
        }
        public HttpCacheConfig DefCacheSet
        {
            get;
            set;
        } = new HttpCacheConfig
        {
            SMaxAge = 3600,
            CacheType = HttpCacheType.Public,
            MaxAge = 3600,
            EnableEtag = true,
            MustRevalidate = false
        };
        public Dictionary<string, HttpCacheConfig> Caches
        {
            get;
            set;
        }

        internal string GetRegex ()
        {
            if (this.Extension.Length == 1)
            {
                return string.Format("^{0}(/.+)*/.+[.]{1}$", this.VirtualPath == "/" ? string.Empty : this.VirtualPath, this.Extension[0]);
            }
            else
            {
                return string.Format("^{0}(/.+)*/.+[.]({1})$", this.VirtualPath == "/" ? string.Empty : this.VirtualPath, string.Join('|', this.Extension));
            }
        }

        internal void InitConfig (ICrossConfig cross)
        {
            if (this.Extension.IsNull())
            {
                this.Extension = new string[]
                {
                        "jpg",
                        "gif",
                        "png",
                        "bmp",
                         "ico",
                        "json",
                        "js",
                        "css",
                       "txt",
                        "html",//html
                        "mp4",
                        "mp3",//媒体文件
                        "woff",
                        "woff2",
                        "eot",//字体
                };
            }
            else
            {
                this.Extension = this.Extension.ConvertAll(x => x[0] == '.' ? x.Remove(0, 1) : x);
            }
            if (this.VirtualPath != "/" && this.VirtualPath.EndsWith('/'))
            {
                this.VirtualPath = this.VirtualPath.Remove(this.VirtualPath.Length - 1, 1);
            }
            if (this.DirPath.IsNull())
            {
                this.DirPath = AppDomain.CurrentDomain.BaseDirectory;
            }
            else if (!Path.IsPathRooted(this.DirPath))
            {
                StringBuilder path = new StringBuilder(this.DirPath);
                if (path[0] == '\\')
                {
                    _ = path.Remove(0, 1);
                }
                if (path[path.Length - 1] == '\\')
                {
                    _ = path.Remove(path.Length - 1, 1);
                }
                this.DirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path.ToString());
            }
            else if (this.DirPath.EndsWith('\\'))
            {
                this.DirPath = this.DirPath.Remove(this.DirPath.Length - 1, 1);
            }
            FileDirCollect.AddDir(this, cross);
        }
    }
}
