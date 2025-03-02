using RpcExtend.Service.AutoRetry;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Modular;

namespace RpcExtend.Service
{
    public class ExtendService
    {
        public static void InitService ()
        {
            RpcClient.RpcEvent = new RpcExtendEvent();
            RpcClient.Start();
        }
        private class RpcExtendEvent : RpcEvent
        {
            public override void Load (RpcInitOption option)
            {
                option.LoadModular<ExtendModular>();
                option.Load("RpcExtend.Service");
            }
            public override void ServerInit (IIocService ioc)
            {
                RetryLogSaveService.Init(ioc);
                AutoRetryTaskService.Init(ioc);
            }
            public override void ServerStarted ()
            {
                AutoRetryTaskService.Start();
            }
        }
        public static void CloseService ()
        {
            RpcClient.Close();
        }
    }
}
