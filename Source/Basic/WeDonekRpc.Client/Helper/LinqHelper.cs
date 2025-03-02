using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Helper
{
    internal static class LinqHelper
    {
        public static void InitBroadcast (this IRemoteBroadcast config)
        {
            if (!config.RpcMerId.HasValue && !config.IsCrossGroup)
            {
                config.RpcMerId = RpcClient.CurrentSource.RpcMerId;
            }
        }
    }
}
