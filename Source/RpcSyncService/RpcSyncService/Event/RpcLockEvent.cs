using RpcModel;
using RpcModel.RemoteLock;

using RpcSyncService.Collect;

namespace RpcSyncService.Event
{
        /// <summary>
        /// 远程锁服务
        /// </summary>
        internal class RpcLockEvent : RpcClient.Interface.IRpcApiService
        {
                /// <summary>
                /// 重置锁
                /// </summary>
                /// <param name="obj"></param>
                /// <param name="source"></param>
                public static void ReleaseLock(ReleaseLock obj, MsgSource source)
                {
                        SyncLockCollect.ReleaseLock(obj, source.SourceServerId);
                }
                /// <summary>
                /// 获取锁状态
                /// </summary>
                /// <param name="obj"></param>
                /// <returns></returns>
                public static ApplyLockRes GetLockStatus(GetLockStatus obj)
                {
                        return SyncLockCollect.GetLockStatus(obj);
                }
                /// <summary>
                /// 申请锁
                /// </summary>
                /// <param name="apply"></param>
                /// <param name="source"></param>
                /// <returns></returns>
                public static ApplyLockRes ApplyLock(ApplyLock apply, MsgSource source)
                {
                        return SyncLockCollect.ApplyLock(apply, source.SourceServerId);
                }
        }
}
