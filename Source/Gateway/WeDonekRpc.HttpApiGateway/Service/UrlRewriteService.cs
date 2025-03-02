using System;
using System.Text.RegularExpressions;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpApiGateway.Model;
using WeDonekRpc.HttpService.Collect;

namespace WeDonekRpc.HttpApiGateway.Service
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class UrlRewriteService : IUrlRewriteService
    {
        private readonly IUrlRewriteConfig _Config;

        public UrlRewriteService (IUrlRewriteConfig config)
        {
            this._Config = config;
            this._Config.SetRefresh(this._Init);
        }
        public void Init ()
        {
            if (!this._Config.Rewrite.IsNull())
            {
                this._Init(this._Config.Rewrite);
            }
        }
        private void _Init (UrlRewrite[] obj)
        {
            obj.ForEach(c =>
            {
                if (!c.IsRegex)
                {
                    if (!c.FormAddr.StartsWith("/"))
                    {
                        c.FormAddr = "/" + c.FormAddr.ToLower();
                    }
                }
                if (UrlRewriteCollect.CheckIsExist(c.FormAddr, c.IsRegex, out string to))
                {
                    if (to == c.ToUri)
                    {
                        return;
                    }
                    else if (c.IsRegex)
                    {
                        UrlRewriteCollect.RemoveRegex(c.FormAddr);
                    }
                    else
                    {
                        UrlRewriteCollect.Remove(c.FormAddr);
                    }
                }
                if (c.IsRegex)
                {
                    UrlRewriteCollect.Add(new Regex(c.FormAddr), c.ToUri);
                }
                else
                {
                    UrlRewriteCollect.Add(c.FormAddr, c.ToUri);
                }
            });
            string[] paths = obj.ConvertAll(c => c.FormAddr);
            UrlRewriteCollect.Exclude(paths);
        }

        public void Remove (string path)
        {
            UrlRewriteCollect.Remove(path);
        }
        public void Remove (Regex regex)
        {
            UrlRewriteCollect.Remove(regex);
        }
        public void Add (string path, string endPoint)
        {
            UrlRewriteCollect.Add(path, endPoint);
        }
        public void Add (Regex regex, string endPoint)
        {
            UrlRewriteCollect.Add(regex, endPoint);
        }
        public void Add (string path, Func<Uri, string> filter)
        {
            UrlRewriteCollect.Add(path, filter);
        }
        public void Add (Regex regex, Func<Uri, string> filter)
        {
            UrlRewriteCollect.Add(regex, filter);
        }
    }
}
