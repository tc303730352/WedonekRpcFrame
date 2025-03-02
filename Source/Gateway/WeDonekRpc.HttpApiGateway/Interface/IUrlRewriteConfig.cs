using System;
using WeDonekRpc.HttpApiGateway.Model;

namespace WeDonekRpc.HttpApiGateway.Interface
{
    public interface IUrlRewriteConfig
    {
        UrlRewrite[] Rewrite { get; }

        void SetRefresh (Action<UrlRewrite[]> refRewrite);
    }
}