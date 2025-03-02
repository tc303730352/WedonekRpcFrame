using System.Reflection;
using WeDonekRpc.Client.Attr;
namespace WeDonekRpc.HttpApiGateway.Interface
{
    /// <summary>
    /// HttpApi 
    /// </summary>
    [IgnoreIoc]
    internal interface IHttpApi
    {
        /// <summary>
        /// Api地址
        /// </summary>
        string ApiName { get; }

        MethodInfo Source { get; }
        /// <summary>
        /// Api地址
        /// </summary>
        string ApiUri { get; }
        void ExecApi (IService service);
        void RegApi (IApiRoute route);
    }
}
