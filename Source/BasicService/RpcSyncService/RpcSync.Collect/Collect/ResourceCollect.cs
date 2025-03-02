using RpcSync.DAL;
using RpcSync.Model;
using RpcSync.Model.DB;
using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.IdGenerator;
using WeDonekRpc.ModularModel.Resource.Model;
using WeDonekRpc.SqlSugar;

namespace RpcSync.Collect.Collect
{
    internal class ResourceCollect : IResourceCollect
    {
        private readonly IResourceListDAL _Resource;
        private readonly IResourceShieldDAL _ResourceShieId;
        private readonly ITransactionService _Transaction;
        public ResourceCollect (IResourceListDAL resource,
            IResourceShieldDAL resourceShield,
            ITransactionService transaction)
        {
            this._Transaction = transaction;
            this._ResourceShieId = resourceShield;
            this._Resource = resource;
        }

        public void Sync (long modularId, int ver, ResourceDatum[] lists)
        {
            if (lists.IsNull())
            {
                return;
            }
            lists = lists.Distinct().ToArray();
            ResourceData[] datas = this._Resource.Gets(modularId);
            if (datas.Length == 0)
            {
                this._Add(modularId, ver, lists);
                return;
            }
            List<ResourceDatum> adds = new List<ResourceDatum>(lists.Length);
            List<ResourceListModel> sets = new List<ResourceListModel>(datas.Length);
            lists.ForEach(c =>
            {
                ResourceData data = datas.Find(a => a.ResourcePath == c.ResourcePath);
                if (data == null)
                {
                    adds.Add(c);
                }
                else if (c.FullPath != data.FullPath ||
                data.VerNum != ver ||
                data.ResourceState == ResourceState.失效 ||
                ( data.ResourceShow.IsNull() && !c.ResourceShow.IsNull() ))
                {
                    if (data.ResourceShow.IsNull() && !c.ResourceShow.IsNull())
                    {
                        data.ResourceShow = c.ResourceShow;
                    }
                    data.VerNum = ver;
                    data.ResourceState = ResourceState.正常;
                    data.FullPath = c.FullPath;
                    data.ResourceVer += 1;
                    sets.Add(data.ConvertMap<ResourceData, ResourceListModel>());
                }
            });
            if (adds.Count == 0 && sets.Count == 0)
            {
                return;
            }
            DateTime now = DateTime.Now;
            ResourceListModel[] ad = adds.ConvertMapToArray<ResourceDatum, ResourceListModel>((a, b) =>
            {
                b.AddTime = now;
                b.VerNum = ver;
                b.ModularId = modularId;
                b.ResourceState = ResourceState.正常;
                b.ResourceVer = 1;
                if (b.ResourceShow == null)
                {
                    b.ResourceShow = string.Empty;
                }
                b.Id = IdentityHelper.CreateId();
                return b;
            });
            this._Resource.Sync(ad, sets.ToArray());
        }

        private void _Add (long modularId, int ver, ResourceDatum[] lists)
        {
            DateTime now = DateTime.Now;
            ResourceListModel[] adds = lists.ConvertMap<ResourceDatum, ResourceListModel>((a, b) =>
            {
                b.AddTime = now;
                b.VerNum = ver;
                b.ModularId = modularId;
                b.ResourceState = ResourceState.正常;
                b.ResourceVer = 1;
                if (b.ResourceShow == null)
                {
                    b.ResourceShow = string.Empty;
                }
                b.Id = IdentityHelper.CreateId();
                return b;
            });
            this._Resource.Adds(adds);
        }
        public void Clear ()
        {
            using (ILocalTransaction tran = this._Transaction.ApplyTran())
            {
                long[] ids = this._Resource.ClearResource();
                if (ids.Length > 0)
                {
                    this._ResourceShieId.Delete(ids);
                }
                tran.Commit();
            }
        }
        public void Invalid ()
        {
            InvalidResource[] invalids = this._Resource.GetInvalidResource();
            if (invalids.IsNull())
            {
                return;
            }
            invalids = invalids.FindAll(a => a.CheckIsInvalid());
            if (invalids.IsNull())
            {
                return;
            }
            this._Resource.SetInvalid(invalids);
        }
    }
}
