using System;
using System.Threading;
namespace RpcHelper
{
        internal class WirteLock : IReadWirteLock
        {
                private readonly ReadWriteLockHelper _Lock = null;
                public WirteLock(ReadWriteLockHelper source)
                {
                        this._Lock = source;
                }
                public void Dispose()
                {
                        this._Lock.ExitWrite();
                }

                public bool GetLock()
                {
                        return this._Lock.GetWriteLock();
                }
        }
        internal class ReadLock : IReadWirteLock
        {
                private readonly ReadWriteLockHelper _Lock = null;
                public ReadLock(ReadWriteLockHelper source)
                {
                        this._Lock = source;
                }
                public bool GetLock()
                {
                        return this._Lock.GetReadLock();
                }
                public void Dispose()
                {
                        this._Lock.ExitRead();
                }
        }
        public class ReadWriteLockHelper : IDisposable
        {
                ~ReadWriteLockHelper()
                {
                        this.Dispose();

                }
                private ReaderWriterLockSlim _Lock = new ReaderWriterLockSlim();
                public ReadWriteLockHelper()
                {
                        this.Write = new WirteLock(this);
                        this.Read = new ReadLock(this);
                }
                public IReadWirteLock Write
                {
                        get;
                }

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
                public bool GetReadLock()
                {
                        return this._Lock.TryEnterReadLock(this._OutTime);
                }

                /// <summary>
                /// 释放读锁
                /// </summary>
                public void ExitRead()
                {
                        this._Lock.ExitReadLock();
                }

                /// <summary>
                /// 获得写锁
                /// </summary>
                /// <returns></returns>
                public bool GetWriteLock()
                {
                        return this._Lock.TryEnterWriteLock(this._OutTime);
                }

                /// <summary>
                /// 释放写锁
                /// </summary>
                public void ExitWrite()
                {
                        this._Lock.ExitWriteLock();
                }

                public void Dispose()
                {
                        if (this._Lock != null)
                        {
                                this._Lock.Dispose();
                                this._Lock = null;
                        }
                }
        }
}
