using WeDonekRpc.Helper;
using WeDonekRpc.Helper.IdGenerator;
using WeDonekRpc.Model;
using RpcStore.DAL;
using RpcStore.Model.ExtendDB;
using RpcStore.Model.ResourceShield;
using RpcStore.RemoteModel.ResourceShield.Model;
using SqlSugar;
using WeDonekRpc.SqlSugar;

namespace RpcStore.DAL.Repository
{
    /// <summary>
    /// 接口资源屏蔽
    /// </summary>
    internal class ResourceShieldDAL : IResourceShieldDAL
    {
        private readonly IRpcExtendResource<ResourceShieldModel> _BasicDAL;
        public ResourceShieldDAL ( IRpcExtendResource<ResourceShieldModel> dal )
        {
            this._BasicDAL = dal;
        }
        public void Sync ( ResourceShieldModel[] shieIds )
        {
            ISqlQueue<ResourceShieldModel> queue = this._BasicDAL.BeginQueue();
            shieIds.ForEach(c =>
            {
                if ( c.Id == 0 )
                {
                    c.Id = IdentityHelper.CreateId();
                    queue.Insert(c);
                }
                else
                {
                    queue.Update(c);
                }
            });
            if ( queue.Submit() <= 0 )
            {
                throw new ErrorException("rpc.store.shieId.sync.fail");
            }
        }

        public ResourceShieldModel Find ( string shieKey, string resourcePath )
        {
            return this._BasicDAL.Get(c => c.ShieIdKey == shieKey && c.ResourcePath == resourcePath);
        }

        public ResourceShieldModel[] GetByResourceId ( long resourceId )
        {
            return this._BasicDAL.Gets(c => c.ResourceId == resourceId);
        }
        public ResourceShieldModel Get ( long id )
        {
            return this._BasicDAL.Get(c => c.Id == id);
        }

        public ResourceShieldState[] CheckIsShieId ( long[] resourceId )
        {
            return this._BasicDAL.GroupBy(c => resourceId.Contains(c.ResourceId), c => c.ResourceId, c => new ResourceShieldState
            {
                ResourceId = c.ResourceId,
                BeOverdueTime = SqlFunc.AggregateMax(c.BeOverdueTime)
            });
        }

        public void Delete ( long id )
        {
            if ( !this._BasicDAL.Delete(a => a.Id == id) )
            {
                throw new ErrorException("rpc.store.shieId.delete.fail");
            }
        }
        public void Delete ( long[] id )
        {
            if ( !this._BasicDAL.Delete(a => id.Contains(a.Id)) )
            {
                throw new ErrorException("rpc.store.shieId.delete.fail");
            }
        }
        public ResourceShieldModel[] Query ( ResourceShieldQuery query, IBasicPage paging, out int count )
        {
            paging.InitOrderBy("Id", true);
            return this._BasicDAL.Query(query.ToWhere(), paging, out count);
        }

        public ResourceShieldKeyState[] CheckIsShieId ( long resourceId, string[] shieKey )
        {
            return this._BasicDAL.Gets<ResourceShieldKeyState>(c => c.ResourceId == resourceId && shieKey.Contains(c.ShieIdKey));
        }
    }
}
