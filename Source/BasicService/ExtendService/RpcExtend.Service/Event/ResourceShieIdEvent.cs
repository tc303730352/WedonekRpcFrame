using RpcExtend.Collect;
using RpcExtend.Collect.Model;
using RpcExtend.Model;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using WeDonekRpc.ModularModel.Shield;
using WeDonekRpc.ModularModel.Shield.Model;

namespace RpcExtend.Service.Event
{
    /// <summary>
    /// 资源屏蔽接口
    /// </summary>
    internal class ResourceShieIdEvent : IRpcApiService
    {
        private readonly IResourceShieldCollect _ResourceShield;

        public ResourceShieIdEvent (IResourceShieldCollect resourceShield)
        {
            this._ResourceShield = resourceShield;
        }

        public void SyncShieId (SyncShieId obj)
        {
            this._ResourceShield.Refresh(obj.Path, new ShieIdQuery
            {
                RpcMerId = obj.RpcMerId,
                ServerId = obj.ServerId,
                SystemType = obj.SystemType,
                VerNum = obj.VerNum,
                ShieIdType = obj.ShieldType
            });
        }


        public ShieldDatum CheckIsShieId (CheckIsShieId obj, MsgSource source)
        {
            ResourceShield shield = this._ResourceShield.GetShield(new ShieIdQuery
            {
                RpcMerId = source.RpcMerId,
                ServerId = source.ServerId,
                SystemType = source.SystemType,
                VerNum = source.VerNum,
                ShieIdType = obj.ShieldType
            }, obj.Path);
            if (shield.Id == 0 || shield.BeOverdueTime <= DateTime.Now.ToLong())
            {
                return new ShieldDatum();
            }
            return new ShieldDatum
            {
                IsShieId = true,
                OverTime = shield.BeOverdueTime
            };

        }
    }
}
