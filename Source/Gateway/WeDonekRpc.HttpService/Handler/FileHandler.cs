using WeDonekRpc.HttpService.Helper;
using WeDonekRpc.HttpService.Interface;
using WeDonekRpc.HttpService.Model;
using WeDonekRpc.Helper;
using System;
using System.IO;
using System.Net;

namespace WeDonekRpc.HttpService.Handler
{

    internal class FileHandler : BasicHandler
    {
        private readonly DirConfig _Config;
        private readonly ICrossConfig _Cross;
        public FileHandler (string regex, DirConfig config, short sortNum) : base(regex, sortNum, true)
        {
            this._Cross = config.cross;
            this._Config = config;
        }
        protected virtual string GetFilePath ()
        {
            return FileHelper.GetFilePath(this.Request.Url, this._Config);
        }

        public override void Execute ()
        {
            string path = this.GetFilePath();
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                this.Response.SetHttpStatus(HttpStatusCode.NotFound);
                return;
            }
            FileInfo file = new FileInfo(path);
            if (this.LoadFile(file))
            {
                this.Response.WriteFile(file);
            }
        }
        protected virtual bool LoadFile (FileInfo file)
        {
            HttpCacheSet cache = this._Config.GetCacheSet(file, this.Request.Url);
            this.Response.SetCache(cache);
            return true;
        }
        private bool _CheckEtag (string etag)
        {
            string fileEtag = this._Config.FindEtag(this.Request.Url.LocalPath);
            if (fileEtag.IsNull())
            {
                string filePath = this.GetFilePath();
                if (string.IsNullOrEmpty(filePath))
                {
                    return false;
                }
                FileInfo file = new FileInfo(filePath);
                if (!file.Exists)
                {
                    return false;
                }
                fileEtag = this._Config.ApplyEtag(file, this.Request.Url);
            }
            return fileEtag == etag;
        }
        protected override bool CheckCache (string etag, string toUpdateTime)
        {
            if (!string.IsNullOrEmpty(etag))
            {
                return this._CheckEtag(etag);
            }
            else if (!string.IsNullOrEmpty(etag))
            {
                string filePath = this.GetFilePath();
                if (!string.IsNullOrEmpty(filePath))
                {
                    DateTime time = File.GetLastWriteTime(filePath);
                    DateTime myTime = DateTime.Parse(toUpdateTime);
                    return myTime < time;
                }
            }
            return false;
        }
        protected override bool InitHandler ()
        {
            if (this._Cross.IsEnable)
            {
                this._Cross.CheckCross(this);
                return !this.Response.IsEnd;
            }
            return base.InitHandler();
        }
    }
}
