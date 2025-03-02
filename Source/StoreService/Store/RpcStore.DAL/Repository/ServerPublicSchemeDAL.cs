using RpcStore.Model.DB;
using RpcStore.RemoteModel;
using RpcStore.RemoteModel.ServerPublic.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.IdGenerator;
using WeDonekRpc.Model;
using WeDonekRpc.SqlSugar;

namespace RpcStore.DAL.Repository
{
    internal class ServerPublicSchemeDAL : IServerPublicSchemeDAL
    {
        private readonly IRepository<ServerPublicSchemeModel> _BasicDAL;

        public ServerPublicSchemeDAL (IRepository<ServerPublicSchemeModel> basicDAL)
        {
            this._BasicDAL = basicDAL;
        }
        public void Enable (long id)
        {
            if (!this._BasicDAL.Update(a => a.Status == SchemeStatus.启用, a => a.Id == id))
            {
                throw new ErrorException("rpc.store.public.scheme.enable.fail");
            }
        }
        public void Disable (long id)
        {
            if (!this._BasicDAL.Update(a => a.Status == SchemeStatus.停用, a => a.Id == id))
            {
                throw new ErrorException("rpc.store.public.scheme.disable.fail");
            }
        }
        public ServerPublicScheme[] Query (PublicSchemeQuery query, IBasicPage paging, out int count)
        {
            paging.InitOrderBy("Id", true);
            return this._BasicDAL.Query<ServerPublicScheme>(query.ToWhere(), paging, out count);
        }
        public ServerPublicSchemeModel Get (long id)
        {
            ServerPublicSchemeModel obj = this._BasicDAL.Get(a => a.Id == id);
            if (obj == null)
            {
                throw new ErrorException("rpc.store.public.scheme.not.find");
            }
            return obj;
        }
        public void Delete (ServerPublicSchemeModel scheme)
        {
            ISqlQueue<ServerPublicSchemeModel> queue = this._BasicDAL.BeginQueue();
            queue.Delete(a => a.Id == scheme.Id);
            queue.Delete<ServiceVerConfigModel>(a => a.SchemeId == scheme.Id);
            queue.Delete<ServiceVerRouteModel>(a => a.SchemeId == scheme.Id);
            if (queue.Submit() <= 0)
            {
                throw new ErrorException("rpc.store.public.scheme.delete.fail");
            }
        }
        public void Set (ServerPublicSchemeModel scheme, PublicScheme set)
        {
            List<ServiceVerConfigModel> vers = [];
            List<ServiceVerRouteModel> routes = [];
            if (!set.Vers.IsNull())
            {
                set.Vers.ForEach(c =>
                {
                    ServiceVerConfigModel ver = new ServiceVerConfigModel
                    {
                        Id = IdentityHelper.CreateId(),
                        SchemeId = scheme.Id,
                        SystemTypeId = c.SystemTypeId,
                        VerNum = c.VerNum
                    };
                    c.ToVer.ForEach(v =>
                    {
                        routes.Add(new ServiceVerRouteModel
                        {
                            SchemeId = scheme.Id,
                            ToVerId = v.ToVerId,
                            Id = IdentityHelper.CreateId(),
                            SystemTypeId = v.SystemTypeId,
                            VerId = ver.Id
                        });
                    });
                    vers.Add(ver);
                });
            }
            scheme.LastTime = DateTime.Now;
            scheme.SchemeName = set.SchemeName;
            scheme.SchemeShow = set.SchemeShow;
            ISqlQueue<ServerPublicSchemeModel> queue = this._BasicDAL.BeginQueue();
            queue.Update(scheme);
            queue.Delete<ServiceVerConfigModel>(a => a.SchemeId == scheme.Id);
            queue.Delete<ServiceVerRouteModel>(a => a.SchemeId == scheme.Id);
            if (vers.Count > 0)
            {
                queue.Insert(vers);
                queue.Insert(routes);
            }
            if (queue.Submit() <= 0)
            {
                throw new ErrorException("rpc.store.public.scheme.set.fail");
            }
        }
        public long Add (PublicSchemeAdd add)
        {
            ServerPublicSchemeModel scheme = new ServerPublicSchemeModel
            {
                Id = IdentityHelper.CreateId(),
                SchemeName = add.SchemeName,
                SchemeShow = add.SchemeShow,
                Status = RemoteModel.SchemeStatus.起草,
                AddTime = DateTime.Now,
                LastTime = DateTime.Now,
                RpcMerId = add.RpcMerId,
            };
            if (!add.Vers.IsNull())
            {
                List<ServiceVerConfigModel> vers = [];
                List<ServiceVerRouteModel> routes = [];
                add.Vers.ForEach(c =>
                {
                    ServiceVerConfigModel ver = new ServiceVerConfigModel
                    {
                        Id = IdentityHelper.CreateId(),
                        SchemeId = scheme.Id,
                        SystemTypeId = c.SystemTypeId,
                        VerNum = c.VerNum
                    };
                    c.ToVer.ForEach(v =>
                    {
                        routes.Add(new ServiceVerRouteModel
                        {
                            SchemeId = scheme.Id,
                            ToVerId = v.ToVerId,
                            Id = IdentityHelper.CreateId(),
                            SystemTypeId = v.SystemTypeId,
                            VerId = ver.Id
                        });
                    });
                    vers.Add(ver);
                });
                ISqlQueue<ServerPublicSchemeModel> queue = this._BasicDAL.BeginQueue();
                queue.Insert(scheme);
                queue.Insert(vers);
                queue.Insert(routes);
                if (queue.Submit() <= 0)
                {
                    throw new ErrorException("rpc.store.public.scheme.add.fail");
                }
            }
            else
            {
                this._BasicDAL.Insert(scheme);
            }
            return scheme.Id;
        }

    }
}
