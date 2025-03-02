using WeDonekRpc.Model;
using RpcStore.RemoteModel.ServerGroup;
using RpcStore.RemoteModel.ServerGroup.Model;
using Store.Gatewary.Modular.Interface;

namespace Store.Gatewary.Modular.Services
{
    internal class ServerGroupService : IServerGroupService
    {
        public long AddServerGroup (ServerGroupAdd group)
        {
            return new AddServerGroup
            {
                Group = group,
            }.Send();
        }

        public void CheckGroupTypeVal (string typeVal)
        {
            new CheckGroupTypeVal
            {
                TypeVal = typeVal,
            }.Send();
        }

        public void DeleteServerGroup (long id)
        {
            new DeleteServerGroup
            {
                Id = id,
            }.Send();
        }

        public ServerGroupItem[] GetAllServerGroup ()
        {
            return new GetAllServerGroup().Send();
        }

        public ServerGroupList[] GetGroupAndType (RpcServerType? serverType)
        {
            return new GetServerGroupList
            {
                ServerType = serverType
            }.Send();
        }

        public ServerGroupDatum GetServerGroup (long id)
        {
            return new GetServerGroup
            {
                Id = id,
            }.Send();
        }

        public void SetServerGroup (long id, string name)
        {
            new SetServerGroup
            {
                Id = id,
                Name = name,
            }.Send();
        }

    }
}
