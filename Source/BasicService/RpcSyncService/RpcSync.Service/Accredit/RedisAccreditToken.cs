using RpcSync.Collect;
using RpcSync.Service.Interface;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using WeDonekRpc.ModularModel.Accredit;
using WeDonekRpc.ModularModel.Accredit.Model;
namespace RpcSync.Service.Accredit
{
    [IocName("redis")]
    internal class RedisAccreditToken : IAccreditToken
    {
        private string _CacheKey;
        private const string _CacheName = "Accredit_";

        private readonly IRedisListController _RedisList;
        private readonly IRemoteServerGroupCollect _RemoteGroup;
        private readonly IRedisController _Cache;
        private readonly IClearAccreditQueue _ClearQueue;

        public RedisAccreditToken (IRedisListController redisList,
            IClearAccreditQueue clear,
            IRemoteServerGroupCollect remoteGroup,
            IRedisController redis)
        {
            this._RemoteGroup = remoteGroup;
            this._ClearQueue = clear;
            this._RedisList = redisList;
            this._Cache = redis;
        }

        public string AccreditId
        {
            get;
            private set;
        }
        public string CheckKey => this.Token.CheckKey;

        public AccreditToken Token { get; private set; }

        private void _InitCache ()
        {
            this._CacheKey = _CacheName + this.AccreditId;
        }
        public void Create (AccreditToken token)
        {
            this.AccreditId = token.AccreditId;
            this.Token = token;
            this._InitCache();
            TimeSpan? time = token.GetOverTime();
            if (!time.HasValue)
            {
                throw new ErrorException("accredit.expire");
            }
            else if (!this._Cache.Add(this._CacheKey, token, time.Value))
            {
                throw new ErrorException("accredit.add.error");
            }
            else if (token.PAccreditId.IsNotNull())
            {
                string subKey = "AccreditSub_" + token.PAccreditId;
                try
                {
                    _ = this._RedisList.Append(subKey, this.AccreditId);
                }
                catch (Exception e)
                {
                    _ = this._Cache.Remove(this._CacheKey);
                    throw ErrorException.FormatError(e);
                }
            }
        }
        public AccreditDatum Get ()
        {
            return new AccreditDatum
            {
                RoleType = this.Token.RoleType,
                StateVer = this.Token.StateVer,
                State = this.Token.State,
                Accredit = new AccreditRes
                {
                    AccreditId = this.Token.AccreditId,
                    CheckKey = this.Token.CheckKey,
                    SysGroup = this.Token.SysGroup,
                    ApplyId = this.Token.ApplyId,
                    RpcMerId = this.Token.RpcMerId,
                    Expire = this.Token.Expire
                }
            };
        }
        public void Set (SetAccredit obj)
        {
            this.Token.State = obj.State;
            this.Token.Expire = !obj.Expire.HasValue ? null : DateTime.Now.AddSeconds(obj.Expire.Value);
            this.Token.StateVer += 1;
            this._SaveToken();
        }
        public TimeSpan? Refresh ()
        {
            if (this.Token.Expire.HasValue)
            {
                return null;
            }
            TimeSpan time = AccreditHelper.GetDefAccreditTime();
            if (this._Cache.SetExpire(this._CacheKey, time))
            {
                return time;
            }
            return null;
        }


        private void _SaveToken ()
        {
            TimeSpan? time = this.Token.GetOverTime();
            if (!time.HasValue)
            {
                throw new ErrorException("accredit.Invalid");
            }
            if (!this._Cache.Set<AccreditToken>(this._CacheKey, this.Token, time.Value))
            {
                throw new ErrorException("accredit.set.fail");
            }
            this.Token.Refresh(this._RemoteGroup);
        }
        public bool SetState (string state, long verNum)
        {
            if (verNum != this.Token.StateVer)
            {
                return false;
            }
            using (WeDonekRpc.Client.RemoteLock rlock = WeDonekRpc.Client.RemoteLock.ApplyLock("SetState_" + this.AccreditId, RemoteLockType.排斥锁))
            {
                if (rlock.GetLock())
                {
                    if (verNum != this.Token.StateVer)
                    {
                        rlock.Exit();
                        return false;
                    }
                    this.Token.State = state;
                    this.Token.StateVer += 1;
                    this._SaveToken();
                    rlock.Exit();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool Cancel ()
        {
            if (this.Remove(out string[] keys))
            {
                this.Token.Refresh(this._RemoteGroup);
                if (!keys.IsNull())
                {
                    this._ClearQueue.Add(keys);
                }
                return true;
            }
            return false;
        }
        public bool Remove (out string[] subs)
        {
            if (!this._Cache.Remove(this._CacheKey))
            {
                subs = null;
                return false;
            }
            if (this.Token.PAccreditId.IsNotNull())
            {
                string key = "AccreditSub_" + this.Token.PAccreditId;
                subs = this._RedisList.Gets<string>(key);
                return this._Cache.Remove(key);
            }
            else
            {
                subs = null;
            }
            return true;
        }
        public bool Init (string accreditId)
        {
            this.AccreditId = accreditId;
            this._InitCache();
            if (this._Cache.TryGet(this._CacheKey, out AccreditToken token))
            {
                this.Token = token;
                if (token.Expire.HasValue)
                {
                    return token.Expire.Value > DateTime.Now;
                }
                return true;
            }
            return false;
        }



        public bool CheckPrower (MsgSource source)
        {
            if (source.RpcMerId == this.Token.RpcMerId)
            {
                return true;
            }
            return this._RemoteGroup.CheckIsExists(this.Token.RpcMerId, source);
        }
    }
}
