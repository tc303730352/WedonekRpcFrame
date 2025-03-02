using WeDonekRpc.Model;
using RpcStore.RemoteModel.ServerType;
using RpcStore.RemoteModel.ServerType.Model;
using Store.Gatewary.Modular.Interface;

namespace Store.Gatewary.Modular.Services
{
    internal class ServerTypeService : IServerTypeService
    {
        public long AddServerType(ServerTypeAdd datum)
        {
            return new AddServerType
            {
                Datum = datum,
            }.Send();
        }

        public void CheckServerTypeVal(string typeVal)
        {
            new CheckServerTypeVal
            {
                TypeVal = typeVal,
            }.Send();
        }

        public void DeleteServerType(long id)
        {
            new DeleteServerType
            {
                Id = id,
            }.Send();
        }

        public ServerType GetServerType(long id)
        {
            return new GetServerType
            {
                Id = id,
            }.Send();
        }

        public ServerType[] GetServerTypeByGroup(long groupId)
        {
            return new GetServerTypeByGroup
            {
                GroupId = groupId,
            }.Send();
        }

        public ServerTypeDatum[] QueryServerType(ServerTypeQuery query, IBasicPage paging, out int count)
        {
            return new QueryServerType
            {
                Query = query,
                Index = paging.Index,
                Size = paging.Size,
                NextId = paging.NextId,
                SortName = paging.SortName,
                IsDesc = paging.IsDesc
            }.Send(out count);
        }

        public void SetServerType(long id, ServerTypeSet datum)
        {
            new SetServerType
            {
                Id = id,
                Datum = datum,
            }.Send();
        }

    }
}
