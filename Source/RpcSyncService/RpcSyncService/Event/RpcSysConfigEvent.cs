using RpcModel;

using RpcSyncService.Collect;

namespace RpcSyncService.Event
{
        /// <summary>
        /// 系统配置服务
        /// </summary>
        internal class RpcSysConfigEvent : RpcClient.Interface.IRpcApiService
        {
                /// <summary>
                /// 获取系统配置
                /// </summary>
                /// <param name="source">事件原</param>
                /// <returns></returns>
                public static RemoteSysConfig GetSysConfig(MsgSource source)
                {
                        return SysConfigCollect.GetConfig(source);
                }
        }
}
