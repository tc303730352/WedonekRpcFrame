using RpcSyncService.Model;

using RpcHelper;
namespace RpcSyncService.Collect
{
        internal class SysLogCollect
        {
                private static readonly IDelayDataSave<SysLog> _LogSave = new DelayDataSave<SysLog>(_SaveLog, 2, 10);



                private static void _SaveLog(ref SysLog[] datas)
                {
                        if (!new DAL.SystemLogDAL().InsertSysLog(datas))
                        {
                                throw new ErrorException("sys.log.add.error");
                        }
                }

                public static void AddLog(SysLog[] logs)
                {
                        _LogSave.AddData(logs);
                }
        }
}
