using System;

using RpcClient.Interface;

using RpcHelper;

namespace RpcClient.Tran
{
        /// <summary>
        /// 无参数事务模板
        /// </summary>
        internal class NoParamTranTemplate<Extend> : ITranTemplate
        {
                /// <summary>
                /// 是否回滚的方法
                /// </summary>
                private readonly Action<Extend> _Rollback = null;
                private readonly Type _Type;
                private readonly bool _IsBasicType;
                private readonly Extend _def = default;
                /// <summary>
                /// 事务模板
                /// </summary>
                /// <param name="name">事务名</param>
                /// <param name="action">回滚的方法</param>
                public NoParamTranTemplate(string name, Action<Extend> action)
                {
                        this._Type = typeof(Extend);
                        this._IsBasicType = Tools.IsBasicType(this._Type);
                        this.TranName = name;
                        this._Rollback = action;
                }
                /// <summary>
                /// 事务名
                /// </summary>
                public string TranName
                {
                        get;
                }
                /// <summary>
                /// 回滚
                /// </summary>
                /// <param name="json"></param>
                /// <param name="extend"></param>
                public void Rollback(string json, string extend)
                {
                        if (extend.IsNull())
                        {
                                this._Rollback(this._def);
                                return;
                        }
                        else if (!this._IsBasicType)
                        {
                                this._Rollback(Tools.Json<Extend>(extend));
                                return;
                        }
                        else
                        {
                                object obj = Tools.StringParse(this._Type, extend);
                                this._Rollback((Extend)obj);
                        }
                }
        }
}
