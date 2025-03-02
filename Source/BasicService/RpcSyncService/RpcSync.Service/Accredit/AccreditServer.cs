using RpcSync.Service.Interface;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using WeDonekRpc.Modular.Model;
using WeDonekRpc.ModularModel.Accredit;
using WeDonekRpc.ModularModel.Accredit.Model;

namespace RpcSync.Service.Accredit
{
    public enum AccreditType
    {
        redis = 0,
        db = 1
    }
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class AccreditServer : IAccreditServer
    {
        private readonly IAccreditKeyCollect _AccreditKey;

        private readonly IAccreditCollect _Server;

        private readonly ISyncAccreditQueue _AccreditQueue;

        private readonly IRefreshAccreditQueue _RefreshQueue;
        private readonly AccreditType _AccreditType;

        public AccreditServer (ISysConfig config,
            IAccreditKeyCollect accreditKey,
            IIocService unity,
            IRefreshAccreditQueue refresh,
            ISyncAccreditQueue accredit)
        {
            this._RefreshQueue = refresh;
            this._AccreditKey = accreditKey;
            this._AccreditQueue = accredit;
            this._AccreditType = config.GetConfigVal<AccreditType>("sync:accredit:AccreditType", AccreditType.redis);
            this._Server = unity.Resolve<IAccreditCollect>(this._AccreditType.ToString());
        }
        public ApplyAccreditRes ApplyAccredit (ApplyAccredit apply, MsgSource source)
        {
            AccreditToken token = this._CreateToken(apply, source);
            this._Server.Accredit(token);
            this._AccreditQueue.Add(token);
            return new ApplyAccreditRes
            {
                StateVer = token.StateVer,
                Accredit = new AccreditRes
                {
                    CheckKey = token.CheckKey,
                    AccreditId = token.AccreditId,
                    SysGroup = token.SysGroup,
                    RpcMerId = token.RpcMerId,
                    ApplyId = token.ApplyId,
                    Expire = token.Expire
                }
            }; ;
        }
        public void CancelAccredit (string accreditId, string checkKey)
        {
            if (this._AccreditKey.TryRemove(checkKey, accreditId))
            {
                this._CancelAccredit(accreditId);
            }
        }

        public int CheckAccredit (CheckAccredit obj)
        {
            if (!this._AccreditKey.Check(obj.CheckKey, obj.AccreditId, out int stateVer))
            {
                throw new ErrorException("accredit.Invalid");
            }
            else if (obj.IsRefresh)
            {
                this._RefreshQueue.Add(obj.AccreditId);
            }
            return stateVer;
        }

        public AccreditDatum GetAccredit (string accreditId, MsgSource source)
        {
            if (!this._Server.Get(accreditId, out IAccreditToken token))
            {
                throw new ErrorException("accredit.Invalid");
            }
            else if (!token.CheckPrower(source))
            {
                throw new ErrorException("accredit.no.prower");
            }
            else
            {
                return token.Get();
            }
        }

        public void KickOutAccredit (string checkKey)
        {
            string accreditId = this._AccreditKey.KickOut(checkKey);
            if (accreditId.IsNotNull())
            {
                this._CancelAccredit(accreditId);
            }
        }

        public SetUserStateRes SetUserState (SetUserState obj)
        {
            if (!this._Server.Get(obj.AccreditId, out IAccreditToken accredit))
            {
                throw new ErrorException("accredit.Invalid");
            }
            else
            {
                bool isSuccess = accredit.SetState(obj.UserState, obj.VerNum);
                AccreditToken token = accredit.Token;
                return new SetUserStateRes
                {
                    IsSuccess = isSuccess,
                    StateVer = token.StateVer,
                    UserState = token.State
                };
            }
        }

        public ApplyAccreditRes ToUpdate (SetAccredit obj)
        {
            if (!this._Server.Get(obj.AccreditId, out IAccreditToken accredit))
            {
                throw new ErrorException("accredit.Invalid");
            }
            else
            {
                accredit.Set(obj);
                AccreditToken token = accredit.Token;
                return new ApplyAccreditRes
                {
                    StateVer = token.StateVer,
                    Accredit = new AccreditRes
                    {
                        CheckKey = token.CheckKey,
                        AccreditId = token.AccreditId,
                        SysGroup = token.SysGroup,
                        RpcMerId = token.RpcMerId,
                        Expire = token.Expire,
                        ApplyId = token.ApplyId
                    }
                };
            }
        }

        private AccreditToken _CreateToken (ApplyAccredit apply, MsgSource source)
        {
            if (apply.ParentId.IsNull())
            {
                return new AccreditToken
                {
                    AccreditId = apply.AccreditId,
                    RoleType = apply.RoleType,
                    Expire = apply.Expire.HasValue ? DateTime.Now.AddSeconds(apply.Expire.Value) : null,
                    SysGroup = source.SysGroup,
                    ApplyId = apply.ApplyId,
                    RpcMerId = source.RpcMerId,
                    CheckKey = string.Concat(apply.RoleType, "_", apply.ApplyId).GetMd5(),
                    State = apply.State,
                    SystemType = source.SystemType,
                    StateVer = 1
                };
            }
            else if (!this._Server.Get(apply.ParentId, out IAccreditToken parent))
            {
                throw new ErrorException("accredit.parent.Invalid");
            }
            else
            {
                return _CreateToken(apply, parent.Token, source);
            }
        }
        private static string _MergeState (ApplyAccredit apply, AccreditToken parent)
        {
            if (parent.State.IsNull())
            {
                return apply.State;
            }
            UserState one = parent.State.Json<UserState>();
            UserState two = apply.State.Json<UserState>();
            return two.Merge(one);
        }
        private static AccreditToken _CreateToken (ApplyAccredit apply, AccreditToken parent, MsgSource source)
        {
            if (apply.IsInherit)
            {
                apply.State = apply.State.IsNull() ? parent.State : _MergeState(apply, parent);
            }
            DateTime? time = apply.Expire.HasValue ? DateTime.Now.AddSeconds(apply.Expire.Value) : null;
            if (time.HasValue && parent.Expire.HasValue && time.Value > parent.Expire.Value)
            {
                time = parent.Expire;
            }
            string checkKey = apply.ApplyId.IsNotNull() ? string.Concat(apply.RoleType, "_", apply.ApplyId).GetMd5() : apply.AccreditId;
            return new AccreditToken
            {
                AccreditId = apply.AccreditId,
                RoleType = parent.RoleType,
                Expire = time,
                SysGroup = source.SysGroup,
                ApplyId = apply.ApplyId ?? apply.AccreditId,
                RpcMerId = source.RpcMerId,
                CheckKey = checkKey,
                PAccreditId = parent.AccreditId,
                State = apply.State,
                SystemType = source.SystemType,
                StateVer = 1
            };
        }


        private void _CancelAccredit (string accreditId)
        {
            if (this._Server.Get(accreditId, out IAccreditToken token))
            {
                _ = token.Cancel();
            }
        }

    }
}
