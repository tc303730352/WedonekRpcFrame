using WeDonekRpc.Client;
using WeDonekRpc.Model;
using RpcStore.Collect;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.ServerPublic.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.lmpl
{
    internal class PublicSchemeService : IPublicSchemeService
    {
        private readonly IServerPublicSchemeCollect _Scheme;

        public PublicSchemeService (IServerPublicSchemeCollect scheme)
        {
            this._Scheme = scheme;
        }

        public long Add (PublicSchemeAdd add)
        {
            return this._Scheme.Add(add);
        }

        public long[] CheckRepeat (long? schemeId, Dictionary<long, int> systemType)
        {
            return this._Scheme.CheckRepeat(schemeId, systemType);
        }

        public void Delete (long id)
        {
            ServerPublicSchemeModel scheme = this._Scheme.Get(id);
            this._Scheme.Delete(scheme);
        }

        public bool Disable (long id)
        {
            ServerPublicSchemeModel scheme = this._Scheme.Get(id);
            return this._Scheme.Disable(scheme);
        }

        public bool Enable (long id)
        {
            ServerPublicSchemeModel scheme = this._Scheme.Get(id);
            return this._Scheme.Disable(scheme);
        }


        public PublicScheme Get (long id)
        {
            return this._Scheme.GetDetailed(id);
        }

        public PagingResult<ServerPublicScheme> Query (PublicSchemeQuery query, IBasicPage paging)
        {
            ServerPublicScheme[] schemes = this._Scheme.Query(query, paging, out int count);
            return new PagingResult<ServerPublicScheme>(schemes, count);
        }

        public void Set (long id, PublicScheme set)
        {
            ServerPublicSchemeModel scheme = this._Scheme.Get(id);
            this._Scheme.Set(scheme, set);
        }
    }
}
