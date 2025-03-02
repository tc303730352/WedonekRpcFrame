using AutoTask.Collect;
using AutoTask.Model;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Helper;

namespace AutoTask.Service.DelaySave
{
    internal class TaskItemStatusService
    {
        private static readonly DelayDataSave<SyncItemResult> _SyncStatus = new DelayDataSave<SyncItemResult>(_Save, 2, 10);


        private static void _Save (ref SyncItemResult[] datas)
        {
            using (IocScope scope = RpcClient.Ioc.CreateTempScore())
            {
                ITaskItemCollect taskItem = scope.Resolve<ITaskItemCollect>();
                taskItem.SyncState(datas);
            }
        }

        public static void ExecComplate (SyncItemResult result)
        {
            _SyncStatus.AddData(result);
        }
    }
}
