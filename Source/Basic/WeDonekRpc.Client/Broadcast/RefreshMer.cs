using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Broadcast
{
    /// <summary>
    /// 通知更新集群配置
    /// </summary>
    [IRemoteBroadcast("RefreshMer", "sys.sync", IsCrossGroup = false, IsOnly = false, IsExclude = false)]
    public class RefreshMer : RpcBroadcast
    {
        public long RpcMerId
        {
            get;
            set;
        }
    }
}
