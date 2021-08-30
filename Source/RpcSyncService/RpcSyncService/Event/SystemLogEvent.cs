using RpcModel;
using RpcModel.SysError;

namespace RpcSyncService.Event
{
        /// <summary>
        /// 系统日志模块
        /// </summary>
        internal class SystemLogEvent : RpcClient.Interface.IRpcApiService
        {
                /// <summary>
                /// 保存错误日志
                /// </summary>
                /// <param name="log"></param>
                /// <param name="source"></param>
                public void SaveErrorLog(SaveErrorLog log, MsgSource source)
                {
                        Logic.SysLogLogic.SaveSysLog(log.Errors, source);
                }
        }
}
