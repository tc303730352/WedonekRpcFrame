using RpcSync.Collect;
using RpcSync.Model;
using RpcSync.Model.DB;
using RpcSync.Service.Interface;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using WeDonekRpc.ModularModel.Accredit;
using WeDonekRpc.ModularModel.Accredit.Model;

namespace RpcSync.Service.Accredit
{
    [IocName("db")]
    internal class DbAccreditToken : DbCacheAccredit, IAccreditToken
    {
        private readonly IAccreditTokenCollect _TokenCollect;
        private readonly IRemoteServerGroupCollect _RemoteGroup;
        private readonly IClearAccreditQueue _ClearQueue;

        public AccreditToken Token { get; private set; }
        private DateTime _OverTime;

        public string CheckKey => this.Token.CheckKey;



        public DbAccreditToken (ICacheController cache,
            IAccreditTokenCollect accreditToken,
            IClearAccreditQueue clear,
            IRemoteServerGroupCollect remoteGroup) : base(cache)
        {
            this._ClearQueue = clear;
            this._TokenCollect = accreditToken;
            this._RemoteGroup = remoteGroup;
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
            subs = this._TokenCollect.GetSubId(this.AccreditId);
            if (this._TokenCollect.Delete(this.AccreditId))
            {
                base.RmoveCache();
                return true;
            }
            return false;
        }
        public bool CheckPrower (MsgSource source)
        {
            if (this.Token.RpcMerId == source.RpcMerId)
            {
                return true;
            }
            return this._RemoteGroup.CheckIsExists(this.Token.RpcMerId, source);
        }

        public void Create (AccreditToken token)
        {
            this.Token = token;
            TimeSpan? time = token.GetOverTime();
            if (!time.HasValue)
            {
                throw new ErrorException("accredit.expire");
            }
            AccreditTokenModel add = token.ConvertMap<AccreditToken, AccreditTokenModel>();
            DateTime now = DateTime.Now;
            add.OverTime = now.Add(time.Value);
            add.AddTime = now;
            this._TokenCollect.Add(add);
            this._OverTime = add.OverTime;
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

        public bool Init (string accreditId)
        {
            base.InitAccreditId(accreditId);
            return this._Init();
        }
        private bool _Init ()
        {
            if (base.TryGet(out AccreditTokenDatum token))
            {
                if (token.AccreditId.IsNull() || token.OverTime <= DateTime.Now)
                {
                    return false;
                }
                this._OverTime = token.OverTime;
                this.Token = token.ConvertMap<AccreditTokenDatum, AccreditToken>();
                return true;
            }
            AccreditTokenDatum datum = this._TokenCollect.Get(this.AccreditId);
            if (datum == null)
            {
                datum = new AccreditTokenDatum();
                base.SetCache(datum);
                return false;
            }
            base.SetCache(datum);
            if (datum.OverTime <= DateTime.Now)
            {
                return false;
            }
            this._OverTime = datum.OverTime;
            this.Token = datum.ConvertMap<AccreditTokenDatum, AccreditToken>();
            return true;
        }

        public TimeSpan? Refresh ()
        {
            if (this.Token.Expire.HasValue)
            {
                return null;
            }
            TimeSpan time = AccreditHelper.GetDefAccreditTime();
            this._OverTime = DateTime.Now.Add(time);
            this._TokenCollect.SetOverTime(this.AccreditId, this._OverTime);
            base.RmoveCache();
            return time;
        }


        public void Set (SetAccredit obj)
        {
            this.Token.State = obj.State;
            this.Token.Expire = !obj.Expire.HasValue ? null : DateTime.Now.AddSeconds(obj.Expire.Value);
            this.Token.StateVer += 1;
            _ = this._SaveToken();
        }

        public bool SetState (string state, long verNum)
        {
            this.Token.State = state;
            this.Token.StateVer += 1;
            return this._SaveToken();
        }
        private bool _SaveToken ()
        {
            TimeSpan? time = this.Token.GetOverTime();
            if (!time.HasValue)
            {
                throw new ErrorException("accredit.Invalid");
            }
            this._OverTime = DateTime.Now.Add(time.Value);
            AccreditTokenSet set = this.Token.ConvertMap<AccreditToken, AccreditTokenSet>();
            set.OverTime = this._OverTime;
            if (this._TokenCollect.Set(this.AccreditId, set))
            {
                base.RmoveCache();
                this.Token.Refresh(this._RemoteGroup);
                return true;
            }
            else if (!this._Init())
            {
                throw new ErrorException("accredit.Invalid");
            }
            return false;
        }
    }
}
