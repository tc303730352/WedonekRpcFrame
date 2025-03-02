using System;
using System.Threading;
namespace WeDonekRpc.Helper.Lock
{
    /// <summary>
    /// 锁
    /// </summary>
    public class LockHelper : IDisposable
    {
        private readonly object _LockData = new object();
        private volatile bool _IsExit = true;
        private int _OutTime = 5000;

        [ThreadStatic]
        private bool _IsDispose = false;
        public LockHelper ()
        {
            this._IsDispose = false;
            this._IsExit = true;
        }
        public LockHelper ( int time )
        {
            this._IsDispose = false;
            this._OutTime = time;
            this._IsExit = true;
        }
        /// <summary>
        /// 超时时间
        /// </summary>
        public int OutTime
        {
            get => this._OutTime;
            set => this._OutTime = value;
        }

        private readonly int _WaitOutTime = 2000;

        /// <summary>
        /// 等待超时时间
        /// </summary>
        public int WaitOutTime
        {
            get => this._OutTime;
            set => this._OutTime = value;
        }

        /// <summary>
        /// 获得锁
        /// </summary>
        /// <returns></returns>
        public bool GetLock ()
        {
            if ( Monitor.TryEnter(this._LockData, this._OutTime) )
            {
                this._IsDispose = false;
                this._IsExit = false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 释放锁
        /// </summary>
        public void Exit ()
        {
            Monitor.Exit(this._LockData);
            this._IsExit = true;
        }
        /// <summary>
        ///  释放对象上的锁并阻止当前线程，直到它重新获取该锁。
        /// </summary>
        /// <returns></returns>
        public bool Wait ()
        {
            return Monitor.Wait(this._LockData, this._WaitOutTime);
        }

        /// <summary>
        ///  通知等待队列中的线程锁定对象状态的更改
        /// </summary>
        public void Pulse ()
        {
            Monitor.Pulse(this._LockData);
        }

        public virtual void Dispose ()
        {
            if ( this._IsDispose == false && !this._IsExit )
            {
                this._IsDispose = true;
                this.Exit();
            }
        }
    }
}
