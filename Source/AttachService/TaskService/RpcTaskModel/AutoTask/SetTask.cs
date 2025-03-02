using WeDonekRpc.Client;

namespace RpcTaskModel.AutoTask
{
    [WeDonekRpc.Model.IRemoteConfig("sys.task")]
    public class SetTask : RpcRemote<bool>
    {
        public long Id
        {
            get;
            set;
        }
        public Model.AutoTaskSet Datum
        {
            get;
            set;
        }
    }
}
