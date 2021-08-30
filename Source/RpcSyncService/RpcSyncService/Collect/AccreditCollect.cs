using System;
using System.Collections.Generic;
using System.Linq;

using RpcModel;

using RpcModularModel.Accredit;
using RpcModularModel.Accredit.Model;
using RpcModularModel.Accredit.Msg;

using RpcSyncService.Model;

using RpcHelper;

namespace RpcSyncService.Collect
{
        internal class AccreditCollect
        {
                private static readonly string _AccreditKey = "AccreditKey_";
                private static readonly DataQueueHelper<SyncAccredit> _AccreditQueue = new DataQueueHelper<SyncAccredit>(_SyncAccredit, 100, 100);
                private static readonly DelayDataSave<string> _RefreshQueue = new DelayDataSave<string>(_RefreshAccredit, _FilterData, 60, 100);

                private static void _FilterData(ref string[] datas)
                {
                        datas = datas.Distinct();
                }

                private static void _RefreshAccredit(ref string[] accreditId)
                {
                        List<string> errors = new List<string>();
                        accreditId.ForEach(a =>
                        {
                                if (!AccreditToken.ToUpdate(a))
                                {
                                        errors.Add(a);
                                }
                        });
                        if (errors.Count > 0)
                        {
                                accreditId = errors.ToArray();
                                throw new ErrorException("sync.accredit.refresh.error");
                        }
                }

                public static SetUserStateRes SetUserState(SetUserState obj, MsgSource source)
                {
                        if (!AccreditToken.GetAccredit(obj.AccreditId, out AccreditToken token))
                        {
                                throw new ErrorException("accredit.Invalid");
                        }
                        else if (token.StateVer < obj.VerNum && !obj.IsCover)
                        {
                                return new SetUserStateRes
                                {
                                        IsSuccess = false,
                                        StateVer = obj.VerNum,
                                        UserState = obj.UserState,
                                };
                        }
                        else
                        {
                                token.StateVer += 1;
                                token.State = obj.UserState;
                                if (!token.SaveToken(out bool isSuccess))
                                {
                                        throw new ErrorException("accredit.set.error");
                                }
                                else if (isSuccess)
                                {
                                        _AccreditRefresh(token, source, false);
                                }
                                return new SetUserStateRes
                                {
                                        IsSuccess = isSuccess,
                                        StateVer = token.StateVer,
                                        UserState = isSuccess ? null : token.State
                                };
                        }
                }
                public static ApplyAccreditRes ToUpdateAccredit(SetAccredit obj, MsgSource source)
                {
                        if (!AccreditToken.GetAccredit(obj.AccreditId, out AccreditToken token))
                        {
                                throw new ErrorException("accredit.Invalid");
                        }
                        else
                        {
                                token.AccreditRole = obj.AccreditRole ?? new string[0];
                                token.State = obj.State;
                                token.StateVer = 1;
                                if (!token.SaveToken())
                                {
                                        throw new ErrorException("accredit.set.error");
                                }
                                _AccreditRefresh(token, source, false);
                        }
                        return new ApplyAccreditRes
                        {
                                StateVer = token.StateVer,
                                Accredit = new AccreditRes
                                {
                                        CheckKey = token.CheckKey,
                                        AccreditId = token.AccreditId,
                                        SysGroupId = token.SysGroupId,
                                        RpcMerId = token.RpcMerId
                                }
                        };
                }
                public static AccreditToken ApplyAccreditToken(ApplyAccredit apply, MsgSource source)
                {
                        AccreditToken token = new AccreditToken
                        {
                                AccreditId = Guid.NewGuid().ToString("N"),
                                RoleType = apply.RoleType,
                                SysGroupId = source.SourceGroupId,
                                ApplyId = apply.ApplyId,
                                RpcMerId = source.RpcMerId,
                                CheckKey = string.Concat(apply.RoleType, "_", apply.ApplyId).GetMd5(),
                                AccreditRole = apply.AccreditRole ?? new string[0],
                                State = apply.State,
                                SystemType = source.SystemType,
                                StateVer = 1
                        };
                        if (!token.AddToken())
                        {
                                throw new ErrorException("accredit.add.error");
                        }
                        SyncAccredit obj = new SyncAccredit
                        {
                                AccreditId = token.AccreditId,
                                ApplyKey = token.CheckKey,
                                Source = source
                        };
                        _AccreditQueue.AddQueue(obj);
                        return token;
                }
                public static ApplyAccreditRes ApplyAccredit(ApplyAccredit apply, MsgSource source)
                {
                        AccreditToken token = ApplyAccreditToken(apply, source);
                        return new ApplyAccreditRes
                        {
                                StateVer = token.StateVer,
                                Accredit = new AccreditRes
                                {
                                        CheckKey = token.CheckKey,
                                        AccreditId = token.AccreditId,
                                        SysGroupId = token.SysGroupId,
                                        RpcMerId = token.RpcMerId
                                }
                        };
                }
                private static bool _TryRemove(string checkKey, out string accreditId)
                {
                        string name = string.Concat(_AccreditKey, checkKey);
                        return RpcClient.RpcClient.Cache.TryRemove(name, out accreditId);
                }
                public static bool SetAccreditKey(AccreditToken token, int time)
                {
                        string name = string.Concat(_AccreditKey, token.CheckKey);
                        if (RpcClient.RpcClient.Cache.TryUpdate(name, (a) =>
                        {
                                return a == token.AccreditId ? a : null;
                        }, out string id, new TimeSpan(0, 0, time)))
                        {
                                if (id != token.AccreditId)
                                {
                                        token.Remove();
                                }
                                return true;
                        }
                        return false;
                }

                public static void KickOutAccredit(string checkKey, MsgSource source)
                {
                        if (!_TryRemove(checkKey, out string accreditId))
                        {
                                return;
                        }
                        else if (!AccreditToken.GetAccredit(accreditId, out AccreditToken token))
                        {
                                return;
                        }
                        else
                        {
                                _AccreditRefresh(token, source, true);
                                token.Remove();
                        }
                }
                private static bool _CheckAccreditKey(string checkKey, string accreditId)
                {
                        string key = string.Concat(_AccreditKey, checkKey);
                        if (RpcClient.RpcClient.Cache.TryGet<string>(key, out string val))
                        {
                                return val == accreditId;
                        }
                        return false;
                }
                private static void _DropAccreditKey(string checkKey)
                {
                        string key = string.Concat(_AccreditKey, checkKey);
                        RpcClient.RpcClient.Cache.Remove(key);
                }
                public static void CancelAccredit(string accreditId, string checkKey, MsgSource source)
                {
                        if (_CheckAccreditKey(checkKey, accreditId))
                        {
                                _DropAccreditKey(checkKey);
                                _CancelAccredit(accreditId, source);
                        }
                }
                private static void _CancelAccredit(string accreditId, MsgSource source)
                {
                        if (AccreditToken.GetAccredit(accreditId, out AccreditToken token))
                        {
                                _AccreditRefresh(token, source, true);
                                token.Remove();
                        }
                }
                public static AccreditDatum GetAccredit(string accreditId, MsgSource source)
                {
                        if (!AccreditToken.GetAccredit(accreditId, out AccreditToken token))
                        {
                                throw new ErrorException("accredit.Invalid");
                        }
                        else if (!token.CheckRole(source))
                        {
                                throw new ErrorException("accredit.no.prower");
                        }
                        else
                        {
                                return new AccreditDatum
                                {
                                        ApplyId = token.ApplyId,
                                        RoleType = token.RoleType,
                                        StateVer = token.StateVer,
                                        State = token.State,
                                        Accredit = new AccreditRes
                                        {
                                                AccreditId = accreditId,
                                                CheckKey = token.CheckKey,
                                                SysGroupId = token.SysGroupId
                                        }
                                };
                        }
                }
                public static bool CheckAccredit(CheckAccredit obj, out string error)
                {
                        if (_CheckAccreditKey(obj.CheckKey, obj.AccreditId))
                        {
                                if (obj.IsRefresh)
                                {
                                        _RefreshQueue.AddData(obj.AccreditId);
                                }
                                error = null;
                                return true;
                        }
                        error = "accredit.Invalid";
                        return false;
                }
                private static void _AccreditRefresh(AccreditToken token, MsgSource source, bool isInvalid)
                {
                        new AccreditRefresh
                        {
                                AccreditId = token.AccreditId,
                                IsInvalid = isInvalid
                        }.Send(token.AccreditRole.Add(token.SystemType), source.RpcMerId);
                }
                private static void _SyncAccredit(SyncAccredit obj)
                {
                        int time = AccreditToken.GetAccreditTime();
                        string key = _SyncAccredit(obj.ApplyKey, obj.AccreditId, time);
                        if (key != obj.AccreditId && key != null)
                        {
                                if (AccreditToken.GetAccredit(key, out AccreditToken token))
                                {
                                        token.Remove();
                                        _AccreditRefresh(token, obj.Source, true);
                                }
                        }
                }
                private static string _SyncAccredit(string accreditKey, string accreditId, int time)
                {
                        string name = string.Concat(_AccreditKey, accreditKey);
                        return RpcClient.RpcClient.Cache.AddOrUpdate<string>(name, accreditId, (a, b) =>
                        {
                                return a != b ? b : null;
                        }, new TimeSpan(0, 0, time));
                }
        }
}
