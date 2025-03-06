using System;
using WeDonekRpc.Model.Tran.Model;

namespace WeDonekRpc.Client.Interface
{
    internal interface ICurTranState : ICurTran, IDisposable
    {
        /// <summary>
        /// 是否已销毁
        /// </summary>
        bool IsDispose { get; }
        /// <summary>
        /// 服务区域Id
        /// </summary>
        int RegionId { get; }

        /// <summary>
        /// 集群ID
        /// </summary>
        long RpcMerId { get; }
        /// <summary>
        /// 事务模板
        /// </summary>
        ITranTemplate Template { get; }
        /// <summary>
        /// 事务原
        /// </summary>
        CurTranState Source { get; }

        void BeginTran ();
    }
}