using System;

using RpcSyncService.Model;

namespace RpcSyncService.Collect
{
        public class IdentityAppCollect
        {
                public static IdentityApp GetApp(Guid id)
                {
                        string key = string.Concat("Identity_", id);
                        if (RpcClient.RpcClient.Cache.TryGet(key, out IdentityApp app))
                        {
                                return app;
                        }
                        app = _GetApp(id);
                        RpcClient.RpcClient.Cache.Set(key, app, new TimeSpan(30, 0, 0, 0));
                        return app;
                }
                private static IdentityApp _GetApp(Guid id)
                {
                        IdentityApp app = new DAL.IdentityAppDAL().GetApp(id);
                        if (app == null)
                        {
                                return new IdentityApp
                                {
                                        IsEnable = false
                                };
                        }
                        return app;
                }
        }
}
