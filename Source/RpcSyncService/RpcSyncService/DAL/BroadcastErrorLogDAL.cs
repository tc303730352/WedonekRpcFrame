using RpcSyncService.Model;

using SqlExecHelper;

namespace RpcSyncService.DAL
{
        internal class BroadcastErrorLogDAL : SqlBasicClass
        {
                public BroadcastErrorLogDAL() : base("BroadcastErrorLog", "RpcExtendService")
                {

                }
                public bool AddErrorLog(RemoteErrorLog[] logs)
                {
                        return this.Insert(logs);
                }
        }
}
