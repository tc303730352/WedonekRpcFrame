using System;

using RpcSyncService.Model;

using SqlExecHelper;

using RpcHelper;

namespace RpcSyncService.DAL
{
        public class IdentityAppDAL : SqlBasicClass
        {
                public IdentityAppDAL() : base("IdentityApp", "RpcExtendService")
                {

                }
                public IdentityApp GetApp(Guid id)
                {
                        if (this.GetRow("Id", id, out IdentityApp app))
                        {
                                return app;
                        }
                        throw new ErrorException("identity.app.get.error");
                }
        }
}
