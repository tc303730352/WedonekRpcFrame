using RpcStore.DAL;
using RpcStore.Model.DB;

namespace RpcStore.Collect.lmpl
{
    internal class ServerSignalStateCollect : IServerSignalStateCollect
    {
        private IServerSignalStateDAL _BasicDAL;

        public ServerSignalStateCollect(IServerSignalStateDAL basicDAL)
        {
            _BasicDAL = basicDAL;
        }
        public ServerSignalStateModel[] Gets(long serverId)
        {
            return _BasicDAL.Gets(serverId);
        }
    }
}
