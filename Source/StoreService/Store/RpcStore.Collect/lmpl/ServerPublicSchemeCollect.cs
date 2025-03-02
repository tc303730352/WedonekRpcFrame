using RpcStore.DAL;
using RpcStore.DAL.Model;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.ServerPublic.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace RpcStore.Collect.lmpl
{
    internal class ServerPublicSchemeCollect : IServerPublicSchemeCollect
    {
        private readonly IServerPublicSchemeDAL _Server;
        private readonly IServiceVerConfigDAL _VerConfig;
        private readonly IServiceVerRouteDAL _Route;

        public ServerPublicSchemeCollect (IServerPublicSchemeDAL server,
            IServiceVerConfigDAL verConfig,
            IServiceVerRouteDAL route)
        {
            this._Server = server;
            this._VerConfig = verConfig;
            this._Route = route;
        }
        public long[] CheckRepeat (long? schemeId, Dictionary<long, int> systemType)
        {
            SchemeSystemType[] types = this._VerConfig.GetSchemeType(systemType);
            if (types.IsNull())
            {
                return null;
            }
            else if (schemeId.HasValue)
            {
                types = types.Remove(c => c.SchemeId == schemeId.Value);
            }
            return types.ConvertAll(t => t.SystemTypeId);
        }
        public long Add (PublicSchemeAdd add)
        {
            if (!add.Vers.IsNull())
            {
                SchemeSystemType[] sysType = this._VerConfig.GetSchemeType(add.Vers.ToDictionary(a => a.SystemTypeId, a => a.VerNum));
                if (!sysType.IsNull())
                {
                    throw new ErrorException("rpc.store.public.scheme.repeat", string.Join(',', sysType.ConvertAll(c => c.SystemTypeId)));
                }
            }
            return this._Server.Add(add);
        }
        public PublicScheme GetDetailed (long id)
        {
            ServerPublicSchemeModel scheme = this._Server.Get(id);
            ServiceVerConfigModel[] vers = this._VerConfig.Gets(id);
            ServiceVerRouteModel[] routes = this._Route.Gets(id);
            return new PublicScheme
            {
                SchemeName = scheme.SchemeName,
                SchemeShow = scheme.SchemeShow,
                Vers = vers.ConvertAll(c => new SystemTypeVerScheme
                {
                    SystemTypeId = c.SystemTypeId,
                    VerNum = c.VerNum,
                    ToVer = routes.Convert(a => a.VerId == c.Id, a => new ToSystemTypeVer
                    {
                        ToVerId = a.ToVerId,
                        SystemTypeId = a.SystemTypeId,
                    })
                })
            };
        }
        public void Delete (ServerPublicSchemeModel scheme)
        {
            if (scheme.Status != RemoteModel.SchemeStatus.起草)
            {
                throw new ErrorException("rpc.store.public.scheme.not.allow.delete");
            }
            this._Server.Delete(scheme);
        }

        public bool Disable (ServerPublicSchemeModel scheme)
        {
            if (scheme.Status == RemoteModel.SchemeStatus.停用)
            {
                return false;
            }
            else if (scheme.Status == RemoteModel.SchemeStatus.起草)
            {
                throw new ErrorException("rpc.store.public.scheme.status.error");
            }
            this._Server.Disable(scheme.Id);
            return true;
        }

        public bool Enable (ServerPublicSchemeModel scheme)
        {
            if (scheme.Status == RemoteModel.SchemeStatus.启用)
            {
                return false;
            }
            this._Server.Enable(scheme.Id);
            return true;
        }

        public ServerPublicSchemeModel Get (long id)
        {
            return this._Server.Get(id);
        }

        public ServerPublicScheme[] Query (PublicSchemeQuery query, IBasicPage paging, out int count)
        {
            return this._Server.Query(query, paging, out count);
        }

        public void Set (ServerPublicSchemeModel scheme, PublicScheme set)
        {
            if (scheme.Status == RemoteModel.SchemeStatus.启用)
            {
                throw new ErrorException("rpc.store.public.scheme.enable");
            }
            if (!set.Vers.IsNull())
            {
                SchemeSystemType[] sysType = this._VerConfig.GetSchemeType(set.Vers.ToDictionary(a => a.SystemTypeId, a => a.VerNum));
                sysType = sysType.Remove(a => a.SchemeId == scheme.Id);
                if (!sysType.IsNull())
                {
                    throw new ErrorException("rpc.store.public.scheme.repeat", string.Join(',', sysType.ConvertAll(c => c.SystemTypeId)));
                }
            }
            this._Server.Set(scheme, set);
        }
    }
}
