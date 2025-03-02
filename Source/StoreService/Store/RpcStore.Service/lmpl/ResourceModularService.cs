using LinqKit;
using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using RpcStore.Collect;
using RpcStore.Model.ExtendDB;
using RpcStore.RemoteModel.ResourceModular.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.lmpl
{
    internal class ResourceModularService : IResourceModularService
    {
        private readonly IResourceModularCollect _Modular;
        private readonly IRpcMerCollect _RpcMer;
        private readonly IServerTypeCollect _ServerType;

        public ResourceModularService (IResourceModularCollect modular, IRpcMerCollect rpcMer, IServerTypeCollect serverType)
        {
            this._Modular = modular;
            this._RpcMer = rpcMer;
            this._ServerType = serverType;
        }

        public ResourceModularModel Get (long modularId)
        {
            return this._Modular.Get(modularId);
        }

        public BasicModular[] Gets (long rpcMerId, string systemType)
        {
            return this._Modular.Gets(rpcMerId, systemType);
        }

        public PagingResult<ResourceModularDatum> Query (ModularQuery query, IBasicPage paging)
        {
            ResourceModularModel[] list = this._Modular.Query(query, paging, out int count);
            if (list.Length == 0)
            {
                return new PagingResult<ResourceModularDatum>();
            }
            ResourceModularDatum[] dtos = list.ConvertMap<ResourceModularModel, ResourceModularDatum>();
            Dictionary<string, string> types = this._ServerType.GetNames(list.ConvertAll(a => a.SystemType));
            dtos.ForEach(c =>
            {
                c.SystemTypeName = types.GetValueOrDefault(c.SystemType);
            });
            return new PagingResult<ResourceModularDatum>(count, dtos);
        }

        public void SetRemark (long id, string remark)
        {
            ResourceModularModel modular = this._Modular.Get(id);
            this._Modular.SetRemark(modular, remark);
        }
    }
}
