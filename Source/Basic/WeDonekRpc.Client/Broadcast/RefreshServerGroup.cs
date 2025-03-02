using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Broadcast
{
    [IRemoteBroadcast("RefreshServerGroup", "sys.sync", IsCrossGroup = false, IsOnly = true, IsExclude = true)]
    public class RefreshServerGroup : RpcBroadcast
    {
        public long RpcMerId
        {
            get;
            set;
        }
        public long[] RemoteId
        {
            get;
            set;
        }
    }
}
