using System;

using RpcClient.Interface;

using RpcHelper;

namespace RpcClient.Tran
{
        /// <summary>
        /// 基础事务模板
        /// </summary>
        /// <typeparam name="T">起始参数</typeparam>
        /// <typeparam name="Extend">扩展参数</typeparam>
        internal class BasicTranTemplate<T, Extend> : ITranTemplate where T : class
        {
                private readonly Action<T, Extend> _Rollback = null;
                private readonly Type _Type = null;
                private readonly bool _IsBasicType = false;
                private readonly Extend _def = default;
                public BasicTranTemplate(string name, Action<T, Extend> action)
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
                /// 回滚事务
                /// </summary>
                /// <param name="json">起始数据</param>
                /// <param name="extend">扩展数据</param>
                public void Rollback(string json, string extend)
                {
                        T data = json.Json<T>();
                        if (extend.IsNull())
                        {
                                this._Rollback(data, this._def);
                                return;
                        }
                        else if (!this._IsBasicType)
                        {
                                this._Rollback(data, Tools.Json<Extend>(extend));
                                return;
                        }
                        else
                        {
                                object obj = Tools.StringParse(this._Type, extend);
                                this._Rollback(data, (Extend)obj);
                        }
                }
        }

}
