using WeDonekRpc.Client;
using RpcTaskModel.AutoTask.Model;

namespace RpcTaskModel.AutoTask
{
    [WeDonekRpc.Model.IRemoteConfig("sys.task")]
    public class QueryTask : BasicPage<AutoTaskBasic>
    {
        public TaskQueryParam QueryParam
        {
            get;
            set;
        }
    }
}
