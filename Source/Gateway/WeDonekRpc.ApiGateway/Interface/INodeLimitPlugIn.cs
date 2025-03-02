namespace WeDonekRpc.ApiGateway.Interface
{
    /// <summary>
    /// 请求接口限流插件
    /// </summary>
    public interface INodeLimitPlugIn : IPlugIn
    {
        bool IsLimit (string key);
    }
}