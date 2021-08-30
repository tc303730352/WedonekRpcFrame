using System;

using RpcModel;

using RpcHelper;

namespace RpcSyncService.Model
{
        [Serializable]
        internal class AccreditToken
        {
                private const string _TimeConfigName = "AccreditTime";

                private const int _DefTime = 7200;

                private const string _CacheName = "Accredit_";


                /// <summary>
                /// 验证KEY 授权用户唯一键
                /// </summary>
                public string CheckKey
                {
                        get;
                        set;
                }
                /// <summary>
                /// 请求唯一键
                /// </summary>
                public string ApplyId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 角色类型
                /// </summary>
                public string RoleType
                {
                        get;
                        set;
                }
                /// <summary>
                /// 授权的服务器角色类别(用于跨服务验证)
                /// </summary>
                public string[] AccreditRole { get; set; }

                /// <summary>
                /// 授权ID
                /// </summary>
                public string AccreditId { get; set; }
                /// <summary>
                /// 所属系统组别
                /// </summary>
                public long SysGroupId { get; set; }
                /// <summary>
                /// 服务组商ID
                /// </summary>
                public long RpcMerId { get; set; }
                /// <summary>
                /// 状态
                /// </summary>
                public string State { get; set; }

                private volatile int _StateVer = 0;

                public int StateVer { get => this._StateVer; set => this._StateVer = value; }

                public string SystemType { get; set; }

                public bool AddToken()
                {
                        string name = string.Concat(_CacheName, this.AccreditId);
                        int time = AccreditToken.GetAccreditTime();
                        return RpcClient.RpcClient.Cache.Add(name, this, new TimeSpan(0, 0, time));
                }
                public static bool ToUpdate(string accreditId)
                {
                        string name = string.Concat(_CacheName, accreditId);
                        int time = AccreditToken.GetAccreditTime();
                        if (!RpcClient.RpcClient.Cache.TryUpdate(name, (a) => { return a; }, out AccreditToken token, new TimeSpan(0, 0, time)))
                        {
                                return true;
                        }
                        else if (token == null)
                        {
                                return true;
                        }
                        else
                        {
                                return Collect.AccreditCollect.SetAccreditKey(token, time);
                        }
                }

                public static int GetAccreditTime()
                {
                        return RpcClient.RpcClient.Config.GetConfigVal<int>(_TimeConfigName, _DefTime);
                }
                public bool SaveToken()
                {
                        int time = AccreditToken.GetAccreditTime();
                        string name = string.Concat(_CacheName, this.AccreditId);
                        return RpcClient.RpcClient.Cache.Set(name, this, new TimeSpan(0, 0, time));
                }
                public bool SaveToken(out bool isSuccess)
                {
                        int time = AccreditToken.GetAccreditTime();
                        string name = string.Concat(_CacheName, this.AccreditId);
                        AccreditToken newToken = RpcClient.RpcClient.Cache.AddOrUpdate<AccreditToken>(name, this, (a, b) =>
                        {
                                if (a.StateVer < b.StateVer)
                                {
                                        return b;
                                }
                                return null;
                        }, new TimeSpan(0, 0, time));
                        if (newToken == null)
                        {
                                isSuccess = false;
                                return false;
                        }
                        else if (newToken.Equals(this))
                        {
                                isSuccess = true;
                                return true;
                        }
                        else
                        {
                                isSuccess = false;
                                this.StateVer = newToken.StateVer;
                                this.State = newToken.State;
                                return true;
                        }
                }
                public void Remove()
                {
                        Remove(this.AccreditId);
                }
                public static bool GetAccredit(string accreditId, out AccreditToken token)
                {
                        string name = string.Concat(_CacheName, accreditId);
                        return RpcClient.RpcClient.Cache.TryGet(name, out token);
                }

                internal static void Remove(string key)
                {
                        string name = string.Concat(_CacheName, key);
                        RpcClient.RpcClient.Cache.Remove(name);
                }

                public bool CheckRole(MsgSource source)
                {
                        return this.AccreditRole.IsExists(a => a == source.SystemType || a == source.GroupTypeVal);
                }
        }
}
