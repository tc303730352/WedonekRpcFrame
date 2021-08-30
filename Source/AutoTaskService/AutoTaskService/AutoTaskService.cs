using AutoTaskService.Collect;

using SqlExecHelper;

namespace AutoTaskService
{
        public class AutoTaskService
        {
                public static async void InitService()
                {
                        SqlExecHelper.Config.SqlConfig.SqlExecType = SqlExecType.全部;
                        RpcClient.RpcClient.Load("AutoTaskService");
                        AutoTaskCollect.LoadTask();
                        RpcClient.RpcClient.Start();
                }
        }
}
