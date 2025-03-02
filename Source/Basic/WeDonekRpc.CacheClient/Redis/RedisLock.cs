using System;
using CSRedis;

namespace WeDonekRpc.CacheClient.Redis
{

    /// <summary>
    /// 同步锁
    /// </summary>
    public class RedisLock : IDisposable
    {
        private CSRedisClientLock _Lock;
        /// <summary>
        /// 同步锁
        /// </summary>
        /// <param name="name">锁名</param>
        /// <param name="autoDelay">是否自动续期</param>
        public RedisLock (string name, bool autoDelay = true) : this(name, 10, autoDelay)
        {
        }
        public RedisLock (string name, int expiry, bool autoDelay = true)
        {
            this._AutoDelay = autoDelay;
            this._LockName = RpcCacheService.FormatKey(name);
            this._ExpiryTime = expiry;
        }
        private readonly string _LockName = null;

        private readonly int _ExpiryTime;

        private volatile bool _IsLock = false;
        private readonly bool _AutoDelay = false;

        public bool GetLock ()
        {
            if (!this._IsLock)
            {
                this._Lock = RedisHelper.Lock(this._LockName, this._ExpiryTime, this._AutoDelay);
                if (this._Lock == null)
                {
                    return false;
                }
                this._IsLock = true;
                return true;
            }
            return true;
        }
        //
        // 摘要:
        //     延长锁时间，锁在占用期内操作时返回true，若因锁超时被其他使用者占用则返回false
        //
        // 参数:
        //   milliseconds:
        //     延长的毫秒数
        //
        // 返回结果:
        //     成功/失败
        public bool Delay (int milliseconds)
        {
            return this._Lock.Delay(milliseconds);
        }

        //
        // 摘要:
        //     刷新锁时间，把key的ttl重新设置为milliseconds，锁在占用期内操作时返回true，若因锁超时被其他使用者占用则返回false
        //
        // 参数:
        //   milliseconds:
        //     刷新的毫秒数
        //
        // 返回结果:
        //     成功/失败
        public bool Refresh (int milliseconds)
        {
            return this._Lock.Refresh(milliseconds);
        }
        public bool Exit ()
        {
            if (this._IsLock)
            {
                if (this._Lock.Unlock())
                {
                    this._IsLock = false;
                    return true;
                }
                return false;
            }
            return true;
        }

        public void Dispose ()
        {
            if (this._IsLock)
            {
                this._Lock.Dispose();
            }
        }
    }
}
