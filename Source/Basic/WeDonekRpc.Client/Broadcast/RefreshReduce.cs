using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Broadcast
{
        /// <summary>
        /// 通知更新节点降级熔断配置
        /// </summary>
        [IRemoteBroadcast("RefreshReduce", IsCrossGroup = true, IsOnly = false, IsExclude = false)]
        public class RefreshReduce : RpcBroadcast
        {
                /// <summary>
                /// 节点Id
                /// </summary>
                public long ServerId
                {
                        get;
                        set;
                }
        }
}
