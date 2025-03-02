using WeDonekRpc.Client;
using WeDonekRpc.Model;

namespace WeDonekRpc.ModularModel.IpBlack
{
    /// <summary>
    /// 刷新IP黑名单
    /// </summary>
    [IRemoteConfig("sys.extend", IsReply = false)]
    public class RefreshIpBlack : RpcRemote
    {
        public long RpcMerId
        {
            get;
            set;
        }
        public string SystemType
        {
            get;
            set;
        }
    }
}
