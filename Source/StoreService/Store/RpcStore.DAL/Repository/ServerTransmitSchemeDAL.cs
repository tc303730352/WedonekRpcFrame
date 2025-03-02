using RpcStore.Model.DB;
using RpcStore.Model.TransmitScheme;
using RpcStore.RemoteModel.TransmitScheme.Model;
using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.IdGenerator;
using WeDonekRpc.Model;
using WeDonekRpc.SqlSugar;

namespace RpcStore.DAL.Repository
{
    internal class ServerTransmitSchemeDAL : IServerTransmitSchemeDAL
    {
        private readonly IRepository<ServerTransmitSchemeModel> _BasicDAL;
        private readonly IRepository<ServerTransmitConfigModel> _TransmitDAL;
        public ServerTransmitSchemeDAL (IRepository<ServerTransmitSchemeModel> dal, IRepository<ServerTransmitConfigModel> repository)
        {
            this._TransmitDAL = repository;
            this._BasicDAL = dal;
        }
        public ServerTransmitSchemeModel Get (long id)
        {
            ServerTransmitSchemeModel obj = this._BasicDAL.Get(a => a.Id == id);
            if (obj == null)
            {
                throw new ErrorException("rpc.store.transmit.scheme.null");
            }
            return obj;
        }

        public ServerTransmitScheme GetScheme (long id)
        {
            ServerTransmitSchemeModel res = this.Get(id);
            ServerTransmitScheme scheme = res.ConvertMap<ServerTransmitSchemeModel, ServerTransmitScheme>();
            scheme.Transmits = this._TransmitDAL.Gets<TransmitDto>(a => a.SchemeId == id);
            return scheme;
        }
        public ServerTransmitSchemeModel[] Query (TransmitSchemeQuery query, IBasicPage paging, out int count)
        {
            paging.InitOrderBy("Id", true);
            return this._BasicDAL.Query(query.ToWhere(), paging, out count);
        }
        public void Adds (TransmitAdd[] data)
        {
            DateTime now = DateTime.Now;
            List<ServerTransmitConfigModel> list = [];
            ServerTransmitSchemeModel[] adds = data.ConvertAll(c =>
            {

                ServerTransmitSchemeModel sc = new ServerTransmitSchemeModel
                {
                    AddTime = now,
                    Scheme = c.Scheme,
                    RpcMerId = c.RpcMerId,
                    TransmitType = c.TransmitType,
                    VerNum = c.VerNum,
                    Show = c.Show,
                    SystemTypeId = c.SystemTypeId,
                    Id = IdentityHelper.CreateId(),
                    IsEnable = false
                };
                if (!c.Transmit.IsNull())
                {
                    ServerTransmitConfigModel[] adds = c.Transmit.ConvertMap<TransmitDatum, ServerTransmitConfigModel>();
                    adds.ForEach(a =>
                    {
                        a.Id = IdentityHelper.CreateId();
                        a.SchemeId = sc.Id;
                    });
                    list.AddRange(adds);
                }
                return sc;
            });
            ISqlQueue<ServerTransmitSchemeModel> queue = this._BasicDAL.BeginQueue();
            queue.Insert(adds);
            queue.Insert(list);
            if (queue.Submit() <= 0)
            {
                throw new ErrorException("rpc.store.transmit.item.add.fail");
            }
        }
        public void SyncItem (long schemeId, TransmitDatum[] transmits)
        {
            ServerTransmitConfigModel[] adds = transmits.ConvertAll(a => new ServerTransmitConfigModel
            {
                Id = IdentityHelper.CreateId(),
                SchemeId = schemeId,
                TransmitConfig = a.TransmitConfig,
                ServerCode = a.ServerCode
            });
            long[] ids = this._TransmitDAL.Gets(a => a.SchemeId == schemeId, a => a.Id);
            if (ids.IsNull())
            {
                this._TransmitDAL.Insert(adds);
                return;
            }
            ISqlQueue<ServerTransmitConfigModel> queue = this._TransmitDAL.BeginQueue();
            queue.Delete(a => ids.Contains(a.Id));
            queue.Insert(adds);
            if (queue.Submit() <= 0)
            {
                throw new ErrorException("rpc.store.transmit.item.sync.fail");
            }
        }
        public long Add (TransmitSchemeAdd scheme)
        {
            ServerTransmitSchemeModel add = new ServerTransmitSchemeModel
            {
                AddTime = DateTime.Now,
                Scheme = scheme.Scheme,
                RpcMerId = scheme.RpcMerId,
                TransmitType = scheme.TransmitType,
                VerNum = scheme.VerNum,
                Show = scheme.Show,
                SystemTypeId = scheme.SystemTypeId,
                Id = IdentityHelper.CreateId(),
                IsEnable = false
            };
            this._BasicDAL.Insert(add);
            return add.Id;
        }
        public void Clear (long rpcMerId)
        {
            long[] ids = this._BasicDAL.Gets(a => a.RpcMerId == rpcMerId, c => c.Id);
            if (ids.IsNull())
            {
                return;
            }
            ISqlQueue<ServerTransmitSchemeModel> queue = this._BasicDAL.BeginQueue();
            queue.Delete(a => ids.Contains(a.Id));
            queue.Delete<ServerTransmitConfigModel>(c => ids.Contains(c.SchemeId));
            if (queue.Submit() <= 0)
            {
                throw new ErrorException("rpc.store.transmit.scheme.delete.fail");
            }
        }
        public void Delete (long schemeId)
        {
            ISqlQueue<ServerTransmitSchemeModel> queue = this._BasicDAL.BeginQueue();
            queue.Delete(a => a.Id == schemeId);
            queue.Delete<ServerTransmitConfigModel>(c => c.SchemeId == schemeId);
            if (queue.Submit() <= 0)
            {
                throw new ErrorException("rpc.store.transmit.scheme.delete.fail");
            }
        }
        public void SetScheme (ServerTransmitSchemeModel source, TransmitSchemeSet set)
        {
            if (source.TransmitType != set.TransmitType)
            {
                ISqlQueue<ServerTransmitSchemeModel> queue = this._BasicDAL.BeginQueue();
                queue.Update(set, a => a.Id == source.Id);
                queue.Delete<ServerTransmitConfigModel>(c => c.SchemeId == source.Id);
                if (queue.Submit() <= 0)
                {
                    throw new ErrorException("rpc.store.transmit.scheme.delete.fail");
                }
                return;
            }
            else if (!this._BasicDAL.Update(set, a => a.Id == source.Id))
            {
                throw new ErrorException("rpc.store.transmit.scheme.set.fail");
            }
        }

        public void CheckIsRepeat (long rpcMerId, TransmitSchemeSet data)
        {
            if (this._BasicDAL.IsExist(c => c.RpcMerId == rpcMerId
            && c.SystemTypeId == data.SystemTypeId
            && c.Scheme == data.Scheme))
            {
                throw new ErrorException("rpc.store.transmit.scheme.repeat");
            }
        }
        public void CheckIsRepeatEnable (ServerTransmitSchemeModel scheme)
        {
            if (this._BasicDAL.IsExist(c => c.RpcMerId == scheme.RpcMerId
            && c.SystemTypeId == scheme.SystemTypeId
            && c.Scheme == scheme.Scheme
            && c.VerNum == scheme.VerNum && c.IsEnable))
            {
                throw new ErrorException("rpc.store.transmit.scheme.repeat.enable");
            }
        }
        public void SetIsEnable (long id, bool isEnable)
        {
            if (!this._BasicDAL.Update(a => a.IsEnable == isEnable, a => a.Id == id))
            {
                throw new ErrorException("rpc.store.transmit.scheme.set.fail");
            }
        }

        public bool CheckIsNull (long schemeId)
        {
            return this._TransmitDAL.IsExist(c => c.SchemeId == schemeId) == false;
        }
    }
}
