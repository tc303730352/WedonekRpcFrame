using WeDonekRpc.Client;
using WeDonekRpc.Model;

namespace Wedonek.Demo.RemoteModel.User
{
    /// <summary>
    /// 锁定用户金额
    /// </summary>
    [IRemoteConfig("demo.user")]
    public class LockUserMoney : RpcRemote
    {
        [RemoteLockAttr]
        public long UserId
        {
            get;
            set;
        }
        public int Money
        {
            get;
            set;
        }
    }
}
