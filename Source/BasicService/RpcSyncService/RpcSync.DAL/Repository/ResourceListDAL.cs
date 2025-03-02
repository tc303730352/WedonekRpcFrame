using WeDonekRpc.Helper;
using RpcSync.Model;
using RpcSync.Model.DB;
using SqlSugar;
using WeDonekRpc.SqlSugar;

namespace RpcSync.DAL.Repository
{
    internal class ResourceListDAL : IResourceListDAL
    {
        private readonly IRpcExtendResource<ResourceListModel> _BasicDAL;
        private static readonly string[] _SetIgnore = new string[] { "ModularId", "ResourcePath", "LastTime", "AddTime" };
        public ResourceListDAL (IRpcExtendResource<ResourceListModel> dal)
        {
            this._BasicDAL = dal;
        }
        public long[] ClearResource ()
        {
            DateTime time = HeartbeatTimeHelper.CurrentDate.AddDays(-10);
            long[] ids = this._BasicDAL.Gets(c => c.ResourceState == ResourceState.失效 && c.LastTime <= time, c => c.Id);
            if (ids.IsNull())
            {
                return ids;
            }
            else if (this._BasicDAL.Delete(a => ids.Contains(a.Id, false)))
            {
                return ids;
            }
            return null;
        }
        public InvalidResource[] GetInvalidResource ()
        {
            return this._BasicDAL.GroupBy(c => c.ResourceState == ResourceState.正常, c => new
            {
                c.ModularId,
                c.VerNum
            }, c => new InvalidResource
            {
                ModularId = c.ModularId,
                VerNum = c.VerNum,
                MaxVer = SqlFunc.AggregateMax(c.ResourceVer),
                MinVer = SqlFunc.AggregateMin(c.ResourceVer)
            });
        }
        public void SetInvalid (InvalidResource[] invalids)
        {
            ISugarQueryable<_Resource>[] list = invalids.ConvertAll(c => this._BasicDAL.Queryable.Where(a => a.ModularId == c.ModularId && a.VerNum == c.VerNum && a.ResourceVer == c.MinVer).Select(c => new _Resource
            {
                Id = c.Id
            }));
            long[] ids = this._BasicDAL.Gets<_Resource, long>(list, "Id");
            if (ids.IsNull())
            {
                return;
            }
            else if (!this._BasicDAL.Update(a => new ResourceListModel
            {
                ResourceState = ResourceState.失效,
                LastTime = DateTime.Now
            }, a => ids.Contains(a.Id, false)))
            {
                throw new ErrorException("rpc.resource.invalid.fail");
            }
        }
        public ResourceData[] Gets (long modularId)
        {
            return this._BasicDAL.Gets(c => c.ModularId == modularId, c => new ResourceData
            {
                FullPath = c.FullPath,
                Id = c.Id,
                ResourcePath = c.ResourcePath,
                ResourceShow = c.ResourceShow,
                ResourceState = c.ResourceState,
                ResourceVer = c.ResourceVer,
                VerNum = c.VerNum
            });
        }

        public void Adds (ResourceListModel[] adds)
        {
            this._BasicDAL.Insert(adds);
        }
        private void _Sets (ResourceListModel[] sets)
        {
            if (!this._BasicDAL.Update(sets, _SetIgnore))
            {
                throw new ErrorException("resource.sync.fail");
            }
        }
        public void Sync (ResourceListModel[] adds, ResourceListModel[] sets)
        {
            if (sets.Length == 0)
            {
                this.Adds(adds);
                return;
            }
            else if (adds.Length == 0)
            {
                this._Sets(sets);
                return;
            }
            ISqlQueue<ResourceListModel> queue = this._BasicDAL.BeginQueue();
            queue.Update(sets, _SetIgnore);
            queue.Insert(adds);
            if (queue.Submit() <= 0)
            {
                throw new ErrorException("resource.sync.fail");
            }
        }
        private class _Resource
        {
            public long Id { get; set; }
        }
    }
}
