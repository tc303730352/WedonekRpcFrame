using System;
using System.Threading;

namespace WeDonekRpc.Helper.Lock
{
    /// <summary>
    /// 同步锁
    /// </summary>
    public class SyncLock : IDisposable
    {
        ~SyncLock ()
        {
            this.Dispose();
        }
        public SyncLock ()
        {

        }
        public SyncLock ( int timeOut )
        {
            this._LockTimeOut = timeOut;
        }
        /// <summary>
        /// 锁超时
        /// </summary>
        private readonly int _LockTimeOut = 5000;

        /// <summary>
        /// 琐的状态
        /// </summary>
        private int _LockStatus = _Wait;

        /// <summary>
        /// 等待同步状态
        /// </summary>
        private const int _Wait = 0;

        /// <summary>
        /// 同步中
        /// </summary>
        private const int _Conduct = 1;

        /// <summary>
        /// 同步完成
        /// </summary>
        private const int _Complate = 2;

        /// <summary>
        /// 原子锁
        /// </summary>
        private volatile ManualResetEvent _Lock = new ManualResetEvent(false);

        public void Dispose ()
        {
            if ( this._Lock != null )
            {
                this._Lock.Dispose();
                this._Lock = null;
            }
        }

        /// <summary>
        /// 获取锁
        /// </summary>
        /// <returns>返回是否是第一个线程</returns>
        public bool GetLock ()
        {
            int ls = Interlocked.CompareExchange(ref this._LockStatus, _Conduct, _Wait);
            //当前线程属于自由状态
            if ( ls == _Wait )
            {
                return true;//当前线程属于自由状态
            }
            else if ( ls == _Complate )
            {
                return false;//同步已完成
            }
            else
            {
                this._Lock.WaitOne(this._LockTimeOut);//挂起线程等待同步完成
                return false;
            }
        }
        /// <summary>
        /// 释放锁
        /// </summary>
        public void Exit ()
        {
            if ( Interlocked.CompareExchange(ref this._LockStatus, _Complate, _Conduct) == _Conduct )
            {
                if ( this._Lock != null && this._Lock.Set() )
                {
                    this.Dispose();
                }
            }
        }
        /// <summary>
        /// 重置并重新获取锁
        /// </summary>
        /// <returns>是否重置成功(失败是锁状态未释放)</returns>
        public bool ResetLock ()
        {
            int ls = Interlocked.CompareExchange(ref this._LockStatus, _Wait, _Complate);
            if ( ls == _Conduct )
            {
                this._Lock.WaitOne(this._LockTimeOut);//挂起线程等待同步完成
                return false;
            }
            else if ( ls == _Complate )
            {
                if ( this._Lock == null )
                {
                    this._Lock = new ManualResetEvent(false);
                }
                else
                {
                    this._Lock.Reset();
                }
            }
            return this.GetLock();
        }

        public void Reset ()
        {
            if ( this._Lock == null )
            {
                this._Lock = new ManualResetEvent(false);
            }
            else
            {
                this._Lock.Reset();
            }
            Interlocked.Exchange(ref this._LockStatus, _Wait);
        }
    }
}