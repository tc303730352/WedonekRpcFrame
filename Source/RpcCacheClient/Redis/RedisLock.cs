using System;

using StackExchange.Redis;

namespace RpcCacheClient.Redis
{
        /// <summary>
        /// 同步锁
        /// </summary>
        public class RedisLock : IDisposable
        {
                private static readonly string _FullLockToken = Environment.MachineName;

                private static readonly string _LocalLockToken = RpcCacheService.FormatKey("SysLock");

                public RedisLock(string name, int db = -1, bool isFullLock = false) : this(name, 10, db, isFullLock)
                {
                }
                public RedisLock(string name, int expiry, int db = -1, bool isFullLock = false)
                {
                        this._db = db;
                        this._LockToken = isFullLock ? _FullLockToken : _LocalLockToken;
                        this._LockName = name;
                        this._ExpiryTime = new TimeSpan(0, 0, expiry);
                }
                private readonly string _LockName = null;
                private readonly int _db = -1;
                private readonly string _LockToken = null;
                private readonly TimeSpan _ExpiryTime;
                private volatile bool _IsLock = false;
                private IDatabase _RedisClient = null;
                public bool GetLock()
                {
                        if (!this._IsLock)
                        {
                                this._RedisClient = RedisHelper.GetPublicClient(this._db);
                                if (this._RedisClient.LockTake(this._LockName, this._LockToken, this._ExpiryTime))
                                {
                                        this._IsLock = true;
                                        return true;
                                }
                                return false;
                        }
                        return true;
                }

                public bool Exit()
                {
                        if (this._IsLock)
                        {
                                if (this._RedisClient.LockRelease(this._LockName, this._LockToken))
                                {
                                        this._IsLock = false;
                                        return true;
                                }
                                return false;
                        }
                        return true;
                }

                public void Dispose()
                {
                        if (this._IsLock)
                        {
                                this._RedisClient.LockRelease(this._LockName, this._LockToken);
                        }
                }
        }
}
