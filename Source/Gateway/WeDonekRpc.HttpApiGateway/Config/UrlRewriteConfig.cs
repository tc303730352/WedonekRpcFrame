using System;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper.Config;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpApiGateway.Model;

namespace WeDonekRpc.HttpApiGateway.Config
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class UrlRewriteConfig : IUrlRewriteConfig
    {
        public UrlRewriteConfig (ISysConfig config)
        {
            IConfigSection section = config.GetSection("gateway:urlRewrite");
            section.AddRefreshEvent(this._Refresh);
        }
        private Action<UrlRewrite[]> _RefRewrite;

        public void SetRefresh (Action<UrlRewrite[]> refRewrite)
        {
            this._RefRewrite = refRewrite;
        }
        private void _Refresh (IConfigSection section, string name)
        {
            this.Rewrite = section.GetValue<UrlRewrite[]>();
            if (this._RefRewrite != null)
            {
                this._RefRewrite(this.Rewrite);
            }
        }

        /// <summary>
        /// 重写地址
        /// </summary>
        public UrlRewrite[] Rewrite { get; private set; }
    }
}
