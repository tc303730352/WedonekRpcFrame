using System;

using RpcClient.Interface;

namespace RpcClient.Tran
{
        internal class NoParamTranTemplate : ITranTemplate
        {
                /// <summary>
                /// 是否回滚的方法
                /// </summary>
                private readonly Action<string> _Rollback = null;
                /// <summary>
                /// 事务模板
                /// </summary>
                /// <param name="name">事务名</param>
                /// <param name="action">回滚的方法</param>
                public NoParamTranTemplate(string name, Action<string> action)
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
                        this._Rollback(extend);
                }
        }
}
