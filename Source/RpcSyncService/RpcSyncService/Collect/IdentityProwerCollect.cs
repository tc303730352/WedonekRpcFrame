using System;

using RpcSyncService.Model;

namespace RpcSyncService.Collect
{
        internal class IdentityProwerCollect
        {

                public static IdentityPrower[] GetPrower(Guid appId, long systemTypeId)
                {
                        string key = string.Concat("IdPrower_", appId.ToString("N"), "_", systemTypeId);
                        if (RpcClient.RpcClient.Cache.TryGet(key, out IdentityPrower[] prowers))
                        {
                                return prowers;
                        }
                        prowers = new DAL.IdentityProwerDAL().GetPrower(appId, systemTypeId);
                        RpcClient.RpcClient.Cache.Set(key, prowers, new TimeSpan(31, 0, 0, 0));
                        return prowers;
                }
        }
}
