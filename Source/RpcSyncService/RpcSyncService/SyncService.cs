using RpcSyncService.Collect;

using SqlExecHelper;

namespace RpcSyncService
{
        public class SyncService
        {
                public static void InitService()
                {
                        SqlExecHelper.Config.SqlConfig.SqlExecType = SqlExecType.全部;
                        RpcClient.RpcClient.RpcEvent = new RpcSyncEvent();
                        RpcClient.RpcClient.Config.SetLocalServer(new Config.ConfigServer());
                        DictateNodeCollect.Init();
                        RpcClient.RpcClient.Load("RpcSyncService");
                        RpcClient.RpcClient.Start();
                        ErrorCollect.Init();
                }

                public static void CloseService()
                {
                        RpcClient.RpcClient.Close();
                }
        }
}
