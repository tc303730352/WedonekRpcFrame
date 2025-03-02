using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Broadcast
{
    /// <summary>
    /// 刷新服务器配置
    /// </summary>
    [IRemoteBroadcast("RefreshService", IsCrossGroup = true, IsOnly = false, IsExclude = false)]
    public class RefreshService : RpcBroadcast
    {
        /// <summary>
        /// 刷新的列表
        /// </summary>
        public long ServerId
        {
            get;
            set;
        }
    }
}
