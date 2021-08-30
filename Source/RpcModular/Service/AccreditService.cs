using System;
using System.Collections.Concurrent;
using System.Linq;

using RpcClient.Attr;

using RpcModel;

using RpcModular.Config;
using RpcModular.Domain;
using RpcModular.Model;

using RpcModularModel.Accredit;
using RpcModularModel.Accredit.Model;
using RpcModularModel.Accredit.Msg;

using RpcHelper;
using RpcHelper.TaskTools;

namespace RpcModular.Service
{
        internal class AccreditService
        {
                internal static ConcurrentDictionary<string, UserAccreditDomain> _AccreditList = new ConcurrentDictionary<string, UserAccreditDomain>();

                static AccreditService()
                {
                        TaskManage.AddTask(new TaskHelper("刷新授权信息!", new TimeSpan(0, 0, 10), new Action(_Clear)));
                        RpcClient.RpcClient.Route.AddRoute<AccreditRefresh>("AccreditRefresh", _AccreditRefresh);
                }
                /// <summary>
                /// 
                /// </summary>
                private static void _Clear()
                {
                        if (_AccreditList.IsEmpty)
                        {
                                return;
                        }
                        int time = HeartbeatTimeHelper.HeartbeatTime;
                        string[] keys = _AccreditList.Keys.ToArray();
                        if (keys.Length > 0)
                        {
                                keys.ForEach(a =>
                                {
                                        if (_AccreditList.TryGetValue(a, out UserAccreditDomain data))
                                        {
                                                if (!data.CheckStatus())
                                                {
                                                        _AccreditList.TryRemove(a, out data);
                                                }
                                        }
                                });
                        }
                }

                private static void _AccreditRefresh(AccreditRefresh obj)
                {
                        _AccreditList.TryRemove(obj.AccreditId, out _);
                }

                public void KickOutAccredit(string applyId)
                {
                        new KickOutAccredit
                        {
                                CheckKey = applyId
                        }.Send();
                }
                /// <summary>
                /// 申请临时授权
                /// </summary>
                /// <param name="role">角色列表</param>
                /// <returns>授权码</returns>
                public string ApplyTempAccredit(string[] role)
                {
                        string applyId = string.Concat("Accredit_", RpcClient.RpcClient.ServerId);
                        ApplyAccreditRes res = new ApplyAccredit
                        {
                                ApplyId = applyId,
                                RoleType = ModularConfig.AccreditRoleType,
                                AccreditRole = role
                        }.Send();
                        return _AddAccredit(applyId, res.Accredit, null, res.StateVer);
                }
                public string AddAccredit(string applyId, string[] roleId, UserState state)
                {
                        MsgSource source = RpcClient.RpcClient.CurrentSource;
                        roleId = roleId.Add(source.SystemType);
                        string json = state == null ? null : Tools.Json(state, typeof(UserState));
                        ApplyAccreditRes res = new ApplyAccredit
                        {
                                ApplyId = applyId,
                                RoleType = ModularConfig.AccreditRoleType,
                                AccreditRole = roleId,
                                State = json
                        }.Send();
                        state.BindAccreditId(res.Accredit.AccreditId, source.SourceGroupId, source.RpcMerId);
                        return _AddAccredit(applyId, res.Accredit, json, res.StateVer);
                }

                private static string _AddAccredit(string applyId, AccreditRes res, string state, long stateVer)
                {
                        UserAccreditDomain accredit = new UserAccreditDomain(applyId, stateVer, res, state);
                        accredit = _AccreditList.GetOrAdd(accredit.AccreditId, accredit);
                        return accredit.AccreditId;
                }
                private static UserAccreditDomain _AddAccredit(AccreditDatum datum)
                {
                        UserAccreditDomain accredit = new UserAccreditDomain(datum);
                        accredit = _AccreditList.GetOrAdd(accredit.AccreditId, accredit);
                        return accredit;
                }
                protected static UserAccreditDomain _GetAccredit(string accreditId)
                {
                        if (!_AccreditList.TryGetValue(accreditId, out UserAccreditDomain accredit))
                        {
                                AccreditDatum res = new GetAccredit
                                {
                                        AccreditId = accreditId
                                }.Send();
                                return _AddAccredit(res);
                        }
                        else if (accredit.IsError)
                        {
                                throw new ErrorException(accredit.Error);
                        }
                        return accredit;
                }

                public void ToUpdate(string accreditId, UserState state, string[] roleId)
                {
                        UserAccreditDomain accredit = _GetAccredit(accreditId);
                        accredit.ToUpdate(state, roleId);
                }
                public void CheckAccredit(string accreditId)
                {
                        if (!_AccreditList.TryGetValue(accreditId, out UserAccreditDomain accredit))
                        {
                                AccreditDatum res = new GetAccredit
                                {
                                        AccreditId = accreditId
                                }.Send();
                                accredit = _AddAccredit(res);
                        }
                        if (accredit.IsError)
                        {
                                throw new ErrorException(accredit.Error);
                        }
                }
                public static void Cancel(string accreditId)
                {
                        UserAccreditDomain accredit = _GetAccredit(accreditId);
                        accredit.CancelAccredit();
                }

                internal static void SetUserState(string accreditId, UserState state)
                {
                        UserAccreditDomain accredit = _GetAccredit(accreditId);
                        accredit.SetUserState(state);
                }

                internal static IUserState SetUserState(string accreditId, UserState state, Func<IUserState, IUserState, IUserState> upFun)
                {
                        UserAccreditDomain accredit = _GetAccredit(accreditId);
                        accredit.SetUserState(state, upFun);
                        return accredit.GetUserState(state.GetType());
                }
                public IAccreditService Create<T>() where T : UserState
                {
                        return new AccreditService<T>();
                }

        }
        [ClassLifetimeAttr(ClassLifetimeType.单例)]
        internal class DefAccreditService : AccreditService, IAccreditService
        {
                public DefAccreditService()
                {

                }
                [ThreadStatic]
                private static IUserState _State = null;

                private static readonly Type _StateType = typeof(UserState);

                public IUserState CurrentUser => _State;

                public IUserState GetAccredit(string accreditId)
                {
                        UserAccreditDomain accredit = _GetAccredit(accreditId);
                        _State = accredit.GetUserState(_StateType);
                        return _State;
                }
                public void CancelAccredit(string accreditId)
                {
                        Cancel(accreditId);
                }
        }
        [IgnoreIoc]
        internal class AccreditService<T> : AccreditService, IAccreditService where T : IUserState
        {
                [ThreadStatic]
                private static IUserState _State = null;

                public IUserState CurrentUser => _State;

                private static readonly Type _StateType = typeof(T);
                public IUserState GetAccredit(string accreditId)
                {
                        UserAccreditDomain accredit = _GetAccredit(accreditId);
                        _State = accredit.GetUserState(_StateType);
                        return _State;
                }
                public void CancelAccredit(string accreditId)
                {
                        Cancel(accreditId);
                }
        }
}
