using System;
using System.Collections.Generic;

using Newtonsoft.Json;

using RpcModular.Service;

using RpcHelper;

namespace RpcModular.Model
{
        /// <summary>
        /// 用户状态值
        /// </summary>
        [RpcClient.Attr.IgnoreIoc]
        public class UserState : IAccreditState
        {
                /// <summary>
                /// 授权ID
                /// </summary>
                [JsonIgnore]
                public string AccreditId { get; private set; }
                /// <summary>
                /// 权限集
                /// </summary>
                public string[] Prower { get; set; }
                /// <summary>
                /// 索引状态值数据
                /// </summary>
                /// <param name="name">属性名</param>
                /// <returns>属性值</returns>

                public virtual object this[string name]
                {
                        get
                        {
                                if (name == "AccreditId")
                                {
                                        return this.AccreditId;
                                }
                                else if (this.Param.TryGetValue(name, out StateParam value))
                                {
                                        return value.GetValue();
                                }
                                return null;
                        }
                        set
                        {
                                if (value == null)
                                {
                                        this.Param.Remove(name);
                                }
                                else if (this.Param.ContainsKey(name))
                                {
                                        this.Param[name] = new StateParam(value);
                                }
                                else
                                {
                                        this.Param.Add(name, new StateParam(value));
                                }
                        }
                }
                /// <summary>
                /// 获取自定义属性值
                /// </summary>
                /// <typeparam name="T">数据结构体</typeparam>
                /// <param name="name">属性名</param>
                /// <returns>属性值</returns>
                public T GetValue<T>(string name)
                {
                        if (this.Param.TryGetValue(name, out StateParam value))
                        {
                                return value.GetValue<T>();
                        }
                        return default;
                }
                /// <summary>
                /// 设置自定义属性值
                /// </summary>
                /// <param name="name">属性名</param>
                /// <param name="value">属性值</param>
                public void SetValue(string name, object value)
                {
                        if (value == null)
                        {
                                return;
                        }
                        if (this.Param.ContainsKey(name))
                        {
                                this.Param[name] = new StateParam(value);
                                return;
                        }
                        this.Param.Add(name, new StateParam(value));
                }
                /// <summary>
                /// 扩展参数
                /// </summary>
                public Dictionary<string, StateParam> Param
                {
                        get;
                        set;
                } = new Dictionary<string, StateParam>();
                [JsonIgnore]
                public long SysGroupId { get; private set; }
                [JsonIgnore]
                public long RpcMerId { get; private set; }

                /// <summary>
                /// 保存当前状态
                /// </summary>
                /// <param name="upFun">比对原始状态值</param>
                /// <returns>最新状态值</returns>
                public IUserState SaveState(Func<IUserState, IUserState, IUserState> upFun)
                {
                        return AccreditService.SetUserState(this.AccreditId, this, upFun);
                }
                /// <summary>
                /// 保存当前状态
                /// </summary>
                /// <param name="error">错误码</param>
                /// <returns>是否保存成功</returns>
                public void SaveState()
                {
                        AccreditService.SetUserState(this.AccreditId, this);
                }
                /// <summary>
                /// 取消授权
                /// </summary>
                public void Cancel()
                {
                        AccreditService.Cancel(this.AccreditId);
                }
                /// <summary>
                /// 绑定授权码
                /// </summary>
                /// <param name="accreditId">授权Id</param>
                public void BindAccreditId(string accreditId, long groupId, long rpcMerId)
                {
                        this.SysGroupId = groupId;
                        this.AccreditId = accreditId;
                        this.RpcMerId = rpcMerId;
                }
                /// <summary>
                /// 检查权限
                /// </summary>
                /// <param name="prower">权限值</param>
                /// <returns>是否校验通过</returns>
                public virtual bool CheckPrower(string prower)
                {
                        return string.IsNullOrEmpty(prower) || this.Prower == null || this.Prower.FindIndex(a => a == prower) != -1;
                }
                public virtual string ToJson()
                {
                        return this.ToJson(typeof(UserState));
                }
        }
}
