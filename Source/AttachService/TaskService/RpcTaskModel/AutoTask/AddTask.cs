using WeDonekRpc.Client;
namespace RpcTaskModel.AutoTask
{
    [WeDonekRpc.Model.IRemoteConfig("sys.task")]
    public class AddTask : RpcRemote<long>
    {
        public Model.AutoTaskAdd Datum
        {
            get;
            set;
        }
    }
}
