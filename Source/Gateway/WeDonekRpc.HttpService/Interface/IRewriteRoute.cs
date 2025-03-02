using System;
using WeDonekRpc.HttpService.Rewrite;
namespace WeDonekRpc.HttpService.Interface
{
    internal interface IRewriteRoute
    {
        string Path { get; }
        bool IsRegex { get; }
        string EndPath { get; }
        bool IsFullPath { get; }
        RouteParam GetRouteParam (Uri uri);
        bool IsUsable (string path);
    }
}