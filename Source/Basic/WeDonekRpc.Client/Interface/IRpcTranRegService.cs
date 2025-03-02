using System;

namespace WeDonekRpc.Client.Interface
{
    /// <summary>
    /// 事务注册服务
    /// </summary>
    public interface IRpcTranRegService
    {
        /// <summary>
        /// 注册事务
        /// </summary>
        /// <param name="name">事务名</param>
        /// <param name="rollback">回滚委托</param>
        void RegSagaTran (string name, Action<ICurTran> rollback);
        /// <summary>
        /// 注册事务
        /// </summary>
        /// <typeparam name="T">指令体</typeparam>
        /// <param name="rollback">回滚委托</param>
        void RegSagaTran<T> (Action<ICurTran> rollback) where T : class;
        /// <summary>
        /// 注册Tcc事务
        /// </summary>
        /// <typeparam name="Ev">事务事件</typeparam>
        /// <param name="name">事件名</param>
        void RegTccTran<Ev> (string name) where Ev : IRpcTccEvent;
        /// <summary>
        /// 注册TCC事务
        /// </summary>
        /// <typeparam name="T">指令体</typeparam>
        /// <typeparam name="Ev">事务事件</typeparam>
        void RegTccTran<T, Ev> () where Ev : IRpcTccEvent where T : class;
        /// <summary>
        /// 注册2PC事务
        /// </summary>
        /// <typeparam name="Ev">事务事件</typeparam>
        /// <param name="name">事件名</param>
        void RegTwoPcTran<Ev> (string name) where Ev : IRpcTwoPcTran;
        /// <summary>
        /// 注册2PC事务
        /// </summary>
        /// <typeparam name="T">指令体</typeparam>
        /// <typeparam name="Ev">事务事件</typeparam>
        void RegTwoPcTran<T, Ev> () where Ev : IRpcTwoPcTran where T : class;
    }
}