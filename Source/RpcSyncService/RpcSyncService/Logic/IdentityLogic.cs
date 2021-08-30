using System;

using RpcModel;

using RpcModularModel.Identity.Model;

using RpcSyncService.Collect;
using RpcSyncService.Model;

using RpcHelper;

namespace RpcSyncService.Logic
{
        internal class IdentityLogic
        {
                public static IdentityDatum GetIdentity(Guid id, MsgSource source)
                {
                        IdentityApp app = IdentityAppCollect.GetApp(id);
                        if (!app.IsEnable || app.EffectiveDate < HeartbeatTimeHelper.CurrentDate)
                        {
                                return new IdentityDatum
                                {
                                        IsValid = false
                                };
                        }
                        IdentityPrower[] prowers = IdentityProwerCollect.GetPrower(id, source.SystemTypeId);
                        return new IdentityDatum
                        {
                                IsValid = true,
                                AppName = app.AppName,
                                Path = prowers.Convert(a => a.ResourceType == RpcModularModel.Resource.ResourceType.API接口, a => a.Value),
                                Routes = prowers.Convert(a => a.ResourceType == RpcModularModel.Resource.ResourceType.RPC接口, a => a.Value),
                        };
                }
        }
}
