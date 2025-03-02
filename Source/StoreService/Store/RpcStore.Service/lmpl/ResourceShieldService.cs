using RpcStore.Collect;
using RpcStore.Model.ExtendDB;
using RpcStore.Model.ResourceShield;
using RpcStore.RemoteModel;
using RpcStore.RemoteModel.ResourceShield.Model;
using RpcStore.Service.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using WeDonekRpc.ModularModel.Shield;

namespace RpcStore.Service.lmpl
{
    /// <summary>
    /// 资源屏蔽
    /// </summary>
    internal class ResourceShieldService : IResourceShieldService
    {
        private readonly IResourceShieldCollect _ShieId;
        private readonly IResourceCollect _Resource;
        private readonly IResourceModularCollect _Modular;
        private readonly IRpcMerCollect _RpcMer;
        private readonly IServerCollect _Server;
        private readonly IServerTypeCollect _ServerType;
        public ResourceShieldService (IResourceShieldCollect shield,
            IRpcMerCollect rpcMer,
            IResourceCollect resource,
            IServerCollect server,
            IResourceModularCollect modular,
            IServerTypeCollect serverType)
        {
            this._Modular = modular;
            this._Resource = resource;
            this._Server = server;
            this._RpcMer = rpcMer;
            this._ServerType = serverType;
            this._ShieId = shield;
        }
        public void CancelResourceShieId (long resourceId)
        {
            ResourceShieldModel[] shield = this._ShieId.GetByResourceId(resourceId);
            if (shield.Length == 0)
            {
                return;
            }
            this._ShieId.Delete(shield);
            shield.ForEach(c =>
            {
                this._RefreshShieId(c);
            });
        }
        public void CancelShieId (long id)
        {
            ResourceShieldModel shield = this._ShieId.Get(id);
            this._ShieId.Delete(shield);
            this._RefreshShieId(shield);
        }

        private void _RefreshShieId (ResourceShieldModel shield)
        {
            new SyncShieId
            {
                ServerId = shield.ServerId,
                SystemType = shield.SystemType,
                ShieldType = shield.ShieldType,
                Path = shield.ResourcePath,
                RpcMerId = shield.RpcMerId,
                VerNum = shield.VerNum
            }.Send(shield.RpcMerId, null);
        }
        public void AddShield (ShieldAddDatum datum)
        {
            this._AddShield(datum, 0);
        }
        private void _AddShield (ShieldAddDatum datum, long resourceId)
        {
            Dictionary<string, long> shieKey = [];
            if (datum.ServerId.IsNull())
            {
                shieKey.Add(string.Join('_', datum.RpcMerId, datum.SystemType, 0, datum.VerNum.GetValueOrDefault()).GetMd5(), 0);
            }
            else
            {
                datum.ServerId.ForEach(c =>
                {
                    shieKey.Add(string.Join('_', datum.RpcMerId, datum.SystemType, c, datum.VerNum.GetValueOrDefault()).GetMd5(), c);
                });
            }
            ResourceShieldKeyState[] state = this._ShieId.CheckIsShieId(resourceId, shieKey.Keys.ToArray());
            long overTime = datum.BeOverdueTime.HasValue ? datum.BeOverdueTime.Value.ToLong() : 0;
            if (state.Length == shieKey.Count && state.FindIndex(c => c.BeOverdueTime != overTime) == -1)
            {
                return;
            }
            else if (state.Length > 0)
            {
                state.ForEach(c =>
                {
                    if (c.BeOverdueTime == overTime)
                    {
                        _ = shieKey.Remove(c.ShieKey);
                    }
                });
            }
            ResourceShieldModel[] sets = shieKey.ConvertAll(c =>
            {
                ResourceShieldModel add = new ResourceShieldModel
                {
                    Id = state.Find(a => a.ShieKey == c.Key, a => a.Id),
                    BeOverdueTime = overTime,
                    ServerId = c.Value,
                    VerNum = datum.VerNum.GetValueOrDefault(),
                    ResourceId = resourceId,
                    ResourcePath = datum.ResourcePath,
                    ShieldType = datum.ShieldType,
                    RpcMerId = datum.RpcMerId,
                    ShieIdKey = c.Key,
                    ShieIdShow = datum.ShieIdShow,
                    SystemType = datum.SystemType,
                    SortNum = 1
                };
                if (add.ServerId != 0 && add.VerNum != 0)
                {
                    add.SortNum = 4;
                }
                else if (add.ServerId == 0 && add.VerNum != 0)
                {
                    add.SortNum = 2;
                }
                else if (add.ServerId != 0 && add.VerNum == 0)
                {
                    add.SortNum = 3;
                }
                else
                {
                    add.SortNum = 1;
                }
                return add;
            });
            this._ShieId.Sync(sets);
            sets.ForEach(c =>
            {
                this._RefreshShieId(c);
            });
        }
        public void SyncShieId (ResourceShieldAdd add)
        {
            ResourceListModel resource = this._Resource.Get(add.ResourceId);
            if (resource.ResourceState == ResourceState.失效)
            {
                throw new ErrorException("rpc.store.resource.already.invalid");
            }
            ResourceModularModel modular = this._Modular.Get(resource.ModularId);
            this._AddShield(new ShieldAddDatum
            {
                VerNum = resource.VerNum,
                ServerId = add.ServerId,
                SystemType = modular.SystemType,
                ShieldType = modular.ResourceType == ResourceType.API接口 ? ShieldType.接口 : ShieldType.指令,
                BeOverdueTime = add.BeOverdueTime,
                ResourcePath = resource.ResourcePath,
                RpcMerId = modular.RpcMerId,
                ShieIdShow = add.ShieIdShow
            }, add.ResourceId);
        }

        public PagingResult<ResourceShieldDatum> Query (ResourceShieldQuery query, IBasicPage paging)
        {
            ResourceShieldModel[] shields = this._ShieId.Query(query, paging, out int count);
            if (shields.IsNull())
            {
                return new PagingResult<ResourceShieldDatum>();
            }
            Dictionary<string, string> types = this._ServerType.GetNames(shields.Distinct(a => a.SystemType));
            Dictionary<long, string> services = this._Server.GetNames(shields.Distinct(a => a.ServerId != 0, a => a.ServerId));
            ResourceShieldDatum[] list = shields.ConvertMap<ResourceShieldModel, ResourceShieldDatum>((a, b) =>
            {
                b.SystemTypeName = types.GetValueOrDefault(a.SystemType);
                if (a.ServerId != 0)
                {
                    b.ServerName = services.GetValueOrDefault(a.ServerId);
                }
                b.ApiVer = a.VerNum.FormatVerNum();
                return b;
            });
            return new PagingResult<ResourceShieldDatum>(list, count);
        }


    }
}
