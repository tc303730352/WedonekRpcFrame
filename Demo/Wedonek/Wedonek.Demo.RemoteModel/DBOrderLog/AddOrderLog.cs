using WeDonekRpc.Client;
using WeDonekRpc.Model;

namespace Wedonek.Demo.RemoteModel.DBOrderLog
{
    [IRemoteConfig("demo.user")]
    public class AddOrderLog : RpcRemote<long>
    {
        /// <summary>
        /// 下单用户ID
        /// </summary>
        public long UserId
        {
            get;
            set;
        }

        public long OrderId
        {
            get;
            set;
        }
        public int OrderPrice
        {
            get;
            set;
        }
    }
}
