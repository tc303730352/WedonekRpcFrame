using WeDonekRpc.Client;
using WeDonekRpc.Model;

namespace WeDonekRpc.ModularModel.Accredit.Msg
{
    /// <summary>
    /// 授权信息刷新事件
    /// </summary>
    [IRemoteBroadcast("AccreditRefresh", false, IsCrossGroup = false, IsExclude = true)]
    public class AccreditRefresh : RpcBroadcast
    {
        /// <summary>
        /// 授权码
        /// </summary>
        public string AccreditId
        {
            get;
            set;
        }
        /// <summary>
        /// 是否无效
        /// </summary>
        public bool IsInvalid
        {
            get;
            set;
        }
    }
}
