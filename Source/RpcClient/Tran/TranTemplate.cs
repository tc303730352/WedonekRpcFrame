using System;

using RpcClient.Interface;

using RpcHelper;
namespace RpcClient.Tran
{
        /// <summary>
        /// 事务模板
        /// </summary>
        /// <typeparam name="T">事务起始数据</typeparam>
        internal class TranTemplate<T> : ITranTemplate where T : class
        {
                /// <summary>
                /// 是否回滚的方法
                /// </summary>
                private readonly Action<T, string> _Rollback = null;
                /// <summary>
                /// 事务模板
                /// </summary>
                /// <param name="name">事务名</param>
                /// <param name="action">回滚的方法</param>
                public TranTemplate(string name, Action<T, string> action)
                {
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
                        T data = json.Json<T>();
                        this._Rollback(data, extend);
                }
        }
}
