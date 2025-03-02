using System;
using WeDonekRpc.Model.Tran.Model;

namespace WeDonekRpc.Client.Interface
{
    /// <summary>
    /// 当前事务
    /// </summary>
    public interface ICurTran : IEquatable<ICurTran>
    {
        /// <summary>
        /// 当前事务Id
        /// </summary>
        long TranId { get; }
        /// <summary>
        /// 超时时间(时间戳)
        /// </summary>
        long OverTime { get; }

        CurTranState TranState { get; }
        /// <summary>
        /// 请求体
        /// </summary>
        ITranSource Body { get; }
    }
}