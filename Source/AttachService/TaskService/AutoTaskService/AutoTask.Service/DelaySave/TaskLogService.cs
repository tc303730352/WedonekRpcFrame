using AutoTask.Collect;
using AutoTask.Collect.Model;
using AutoTask.Model.DB;
using AutoTask.Service.Model;
using RpcTaskModel;
using WeDonekRpc.Client;
using WeDonekRpc.Helper;

namespace AutoTask.Service.DelaySave
{
    internal class TaskLogService
    {
        private static readonly DelayDataSave<AutoTaskLogModel> _SaveLog;
        private static readonly IAutoTaskLogCollect _TaskLog;
        static TaskLogService ()
        {
            _TaskLog = RpcClient.Ioc.Resolve<IAutoTaskLogCollect>();
            _SaveLog = new DelayDataSave<AutoTaskLogModel>(_Save, 2, 10);
        }

        private static void _Save (ref AutoTaskLogModel[] datas)
        {
            _TaskLog.Adds(datas);
        }

        public static void Adds (TaskExecLog[] logs, TaskItemDto item, long taskId)
        {
            logs.ForEach(a =>
            {
                if (a.logRange == TaskLogRange.关闭日志 ||
                ( a.isFail && ( TaskLogRange.记录错误 & a.logRange ) != TaskLogRange.记录错误 ) ||
                ( !a.isFail && ( TaskLogRange.记录成功 & a.logRange ) != TaskLogRange.记录成功 ))
                {
                    return;
                }
                AutoTaskLogModel log = new AutoTaskLogModel()
                {
                    ItemId = item.Id,
                    ItemTitle = item.ItemTitle,
                    EndTime = a.end,
                    BeginTime = a.begin,
                    Error = a.error,
                    IsFail = a.isFail,
                    TaskId = taskId,
                    Result = a.result
                };
                _SaveLog.AddData(log);
            });
        }
    }
}
