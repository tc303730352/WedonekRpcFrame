using WeDonekRpc.Model;
using RpcStore.DAL;
using RpcStore.Model.DB;

namespace RpcStore.Collect.lmpl
{
    internal class ServerRunStateCollect : IServerRunStateCollect
    {
        private IServerRunStateDAL _BasicDAL;

        public ServerRunStateCollect(IServerRunStateDAL basicDAL)
        {
            _BasicDAL = basicDAL;
        }
        public ServerRunStateModel Get(long serverId)
        {
            return this._BasicDAL.Get(serverId);
        }

        public ServerRunStateModel[] Query(IBasicPage paging, out int count)
        {
            return this._BasicDAL.Query(paging, out count);
        }
    }
}
