
using System;

using RpcSyncService.Model;

using SqlExecHelper;

using RpcHelper;

namespace RpcSyncService.DAL
{
        internal class IdentityProwerDAL : SqlBasicClass
        {
                public IdentityProwerDAL() : base("IdentityPrower", "RpcExtendService")
                {

                }
                public IdentityPrower[] GetPrower(Guid appId, long systemTypeId)
                {
                        if (this.Get(out IdentityPrower[] paths, new ISqlWhere[] {
                                new SqlWhere("RoleId", System.Data.SqlDbType.UniqueIdentifier){Value=appId},
                                new SqlWhere("SystemTypeId", System.Data.SqlDbType.BigInt){Value=systemTypeId}
                        }))
                        {
                                return paths;
                        }
                        throw new ErrorException("identity.prower.get.error");
                }
        }
}
