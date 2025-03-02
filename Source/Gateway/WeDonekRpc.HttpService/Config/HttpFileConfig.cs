using System;
using System.Collections.Generic;
using System.IO;
using WeDonekRpc.HttpService.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Http;

namespace WeDonekRpc.HttpService.Config
{
    public class HttpFileConfig
    {
        public HttpFileConfig ()
        {
        }
        /// <summary>
        /// 临时文件目录
        /// </summary>
        public string TempDirPath
        {
            get;
            set;
        } = "TempFile";
        /// <summary>
        /// 文件目录配置
        /// </summary>
        public FileDirConfig[] DirConfig
        {
            get;
            set;
        }
        /// <summary>
        /// 媒体类型注册
        /// </summary>
        public Dictionary<string, string> ContentType
        {
            get;
            set;
        }

        public void Init (ICrossConfig cross)
        {
            if (this.ContentType != null && this.ContentType.Count != 0)
            {
                HttpHeaderHelper.RegContentType(this.ContentType);
            }
            if (this.TempDirPath.IsNull())
            {
                this.TempDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TempFile");
            }
            else if (!Path.IsPathRooted(this.TempDirPath))
            {
                this.TempDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this.TempDirPath);
            }
            if (!this.DirConfig.IsNull())
            {
                this.DirConfig.ForEach(c =>
                {
                    c.InitConfig(cross);
                });
            }
        }
    }
}
