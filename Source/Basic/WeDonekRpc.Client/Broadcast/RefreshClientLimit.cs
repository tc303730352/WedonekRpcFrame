using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Broadcast
{
        /// <summary>
        /// 通知更新节点限流配置
        /// </summary>
        [IRemoteBroadcast("RefreshClientLimit", IsCrossGroup = true, IsOnly = false, IsExclude = false)]
        public class RefreshClientLimit : RpcBroadcast
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
