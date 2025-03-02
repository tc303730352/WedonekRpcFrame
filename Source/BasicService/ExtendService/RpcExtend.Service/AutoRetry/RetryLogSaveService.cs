using RpcExtend.Collect;
using RpcExtend.Model.DB;
using RpcExtend.Model.RetryTask;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.ExtendModel;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.IdGenerator;
using WeDonekRpc.Helper.Interface;

namespace RpcExtend.Service.AutoRetry
{
    internal class RetryLogSaveService
    {
        private static readonly IDelayDataSave<AutoRetryTaskLogModel> _delayData = new DelayDataSave<AutoRetryTaskLogModel>(_SaveLog, 1, 10);
        private static IIocService _Ioc;
        private static void _SaveLog (ref AutoRetryTaskLogModel[] datas)
        {
            using (IocScope scope = _Ioc.CreateScore())
            {
                IAutoRetryTaskLogCollect log = scope.Resolve<IAutoRetryTaskLogCollect>();
                log.Adds(datas);
            }
        }
        public static void Init (IIocService ioc)
        {
            _Ioc = ioc;
        }
        public static void Add (RetryTask task, RetryTaskResult result, int duration, DateTime now)
        {
            _delayData.AddData(new AutoRetryTaskLogModel
            {
                Id = IdentityHelper.CreateId(),
                Duration = duration,
                TaskId = task.Id,
                RetryNum = task.RetryNum,
                RunTime = now,
                ErrorCode = result.ErrorCode,
                IsFail = result.Status == AutoRetryTaskStatus.已重试失败,
                ServerId = RpcClient.ServerId
            });
        }
    }
}
