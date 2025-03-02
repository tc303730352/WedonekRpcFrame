using RpcSync.Service.Interface;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Model;
using WeDonekRpc.Model.RemoteLock;
namespace RpcSync.Service.Event
{
    /// <summary>
    /// 远程锁服务
    /// </summary>
    internal class RpcLockEvent : IRpcApiService
    {
        private readonly ISyncLockService _Service;

        public RpcLockEvent (ISyncLockService service)
        {
            this._Service = service;
        }


        /// <summary>
        /// 重置锁
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="source"></param>
        public void ReleaseLock (ReleaseLock obj, MsgSource source)
        {
            this._Service.ReleaseLock(obj, source.ServerId);
        }
        /// <summary>
        /// 获取锁状态
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ApplyLockRes GetLockStatus (GetLockStatus obj)
        {
            return this._Service.GetLockStatus(obj);
        }
        /// <summary>
        /// 锁续期
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="source"></param>
        public void LockRenewal (LockRenewal obj, MsgSource source)
        {
            this._Service.LockRenewal(obj, source.ServerId);
        }
        /// <summary>
        /// 申请锁
        /// </summary>
        /// <param name="apply"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public ApplyLockRes ApplyLock (ApplyLock apply, MsgSource source)
        {
            return this._Service.ApplyLock(apply, source.ServerId);
        }
    }
}
