using AutoTask.Service.Service;
using WeDonekRpc.Client;
using WeDonekRpc.Modular;

namespace AutoTask.Service
{
    public class AutoTaskService
    {
        public static void InitService ()
        {
            RpcClient.InitComplate += RpcClient_InitComplate;
            RpcClient.Start((option) =>
            {
                option.LoadModular<ExtendModular>();
                option.Load("AutoTask.Service");
            });
        }

        private static void RpcClient_InitComplate ()
        {
            TaskService.Init();
        }
    }
}
