using RpcModel;

namespace RpcClient.Broadcast
{
        /// <summary>
        /// 通知更新集群配置
        /// </summary>
        [IRemoteBroadcast("RefreshMer", "sys.sync", IsCrossGroup = true, IsOnly = false, IsExclude = false)]
        public class RefreshMer : RpcBroadcast
        {
                public long RpcMerId
                {
                        get;
                        set;
                }
        }
}
