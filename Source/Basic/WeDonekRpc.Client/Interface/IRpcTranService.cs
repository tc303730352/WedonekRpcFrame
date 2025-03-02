using System.Collections.Generic;
using WeDonekRpc.Model;
using WeDonekRpc.Model.Tran.Model;

namespace WeDonekRpc.Client.Interface
{
    /// <summary>
    /// 事务基础服务
    /// </summary>
    public interface IRpcTranService
    {
        /// <summary>
        /// 是否处在事务中
        /// </summary>
        bool IsExecTran { get; }
        /// <summary>
        /// 获取事务状态
        /// </summary>
        /// <returns></returns>
        TransactionStatus GetTranState ();
        /// <summary>
        /// 获取一个事务的状态
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        bool GetTranState (CurTranState state, out TransactionStatus status, out string error);
        /// <summary>
        /// 设置事务扩展
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="extend"></param>
        void SetTranExtend<T> (T extend) where T : class;
        /// <summary>
        /// 设置事务扩展
        /// </summary>
        /// <param name="extend"></param>
        void SetTranExtend (string extend);
        /// <summary>
        /// 设置事务扩展
        /// </summary>
        /// <param name="extend"></param>
        void SetTranExtend (Dictionary<string, object> extend);
    }
}