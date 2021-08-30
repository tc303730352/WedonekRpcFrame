using System;

using RpcModularModel.Resource.Model;

using RpcSyncService.Model;

using RpcHelper;

namespace RpcSyncService.Collect
{
        internal class ResourceCollect : BasicCollect<DAL.ResourceListDAL>
        {
                public static void Sync(Guid modularId, string ver, ResourceDatum[] lists)
                {
                        BasicDAL.Sync(modularId, ver, lists);
                }
                public static void ClearResource()
                {
                        BasicDAL.ClearResource();
                }
                public static void InvalidResource()
                {
                        InvalidResource[] invalids = BasicDAL.GetInvalidResource();
                        if (invalids.IsNull())
                        {
                                return;
                        }
                        invalids = invalids.FindAll(a => a.CheckIsInvalid());
                        if (invalids.IsNull())
                        {
                                return;
                        }
                        BasicDAL.Set(invalids);
                }
        }
}
