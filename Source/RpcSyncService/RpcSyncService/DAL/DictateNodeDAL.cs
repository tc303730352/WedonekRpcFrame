using RpcSyncService.Model;

namespace RpcSyncService.DAL
{
        internal class DictateNodeDAL : SqlExecHelper.SqlBasicClass
        {
                public DictateNodeDAL() : base("DictateNode", "RpcExtendService")
                {

                }
                public bool GetDictateNode(out DictateNode[] nodes)
                {
                        return this.Get(out nodes);
                }
        }
}
