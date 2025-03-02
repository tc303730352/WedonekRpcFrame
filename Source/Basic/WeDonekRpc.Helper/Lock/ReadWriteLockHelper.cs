using System;
using System.Threading;
namespace WeDonekRpc.Helper.Lock
{
    internal class WirteLock : IReadWirteLock
    {
        private volatile bool _IsLock = false;
        private readonly ReadWriteLockHelper _Lock = null;
        public WirteLock ( ReadWriteLockHelper source )
        {
            this._Lock = source;
        }
        public void Dispose ()
        {
            if ( _IsLock )
            {
                this._Lock.ExitWrite();
                _IsLock = false;
            }
        }

        public bool GetLock ()
        {
            _IsLock = this._Lock.GetWriteLock();
            return _IsLock;
        }
    }
    internal class ReadLock : IReadWirteLock
    {
        [ThreadStatic]
        private static bool _IsLock = false;
        private readonly ReadWriteLockHelper _Lock = null;
        public ReadLock ( ReadWriteLockHelper source )
        {
            this._Lock = source;
        }
        public bool GetLock ()
        {
            _IsLock = this._Lock.GetReadLock();
            return _IsLock;
        }
        public void Dispose ()
        {
            if ( _IsLock )
            {
                this._Lock.ExitRead();
                _IsLock = false;
            }
        }
    }
    /// <summary>
    /// 读写锁
    /// </summary>
    public class ReadWriteLockHelper : IDisposable
    {
        ~ReadWriteLockHelper ()
        {
            this.Dispose();

        }
        private ReaderWriterLockSlim _Lock = new ReaderWriterLockSlim();
        public ReadWriteLockHelper ()
        {
            this.Write = new WirteLock(this);
            this.Read = new ReadLock(this);
        }
        /// <summary>
        /// 写锁
        /// </summary>
        public IReadWirteLock Write
        {
            get;
        }
        /// <summary>
        /// 读锁
        /// </summary>
        public IReadWirteLock Read
        {
            get;
        }

        private int _OutTime = 2000;

        /// <summary>
        /// 超时时间
        /// </summary>
        public int OutTime
        {
            get => this._OutTime;
            set => this._OutTime = value;
        }

        /// <summary>
        /// 获得读锁
        /// </summary>
        /// <returns></returns>
        public bool GetReadLock ()
        {
            return this._Lock.TryEnterReadLock(this._OutTime);
        }

        /// <summary>
        /// 释放读锁
        /// </summary>
        public void ExitRead ()
        {
            this._Lock.ExitReadLock();
        }

        /// <summary>
        /// 获得写锁
        /// </summary>
        /// <returns></returns>
        public bool GetWriteLock ()
        {
            return this._Lock.TryEnterWriteLock(this._OutTime);
        }

        /// <summary>
        /// 释放写锁
        /// </summary>
        public void ExitWrite ()
        {
            this._Lock.ExitWriteLock();
        }

        public void Dispose ()
        {
            if ( this._Lock != null )
            {
                this._Lock.Dispose();
                this._Lock = null;
            }
        }
    }
}
