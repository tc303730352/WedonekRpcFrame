using System;

using RpcClient.Tran;

namespace RpcClient.Interface
{
        public interface IRpcTransaction : IDisposable
        {
                /// <summary>
                /// 当前事务
                /// </summary>
                ICurTran Tran { get; }
                /// <summary>
                /// 设置事务扩展
                /// </summary>
                /// <param name="extend"></param>
                void SetExtend(string extend);
                /// <summary>
                /// 完成事务
                /// </summary>
                void Complate();
                /// <summary>
                /// 完成事务（需干预事务结果时使用）
                /// </summary>
                /// <param name="checkTran">检查事务状态</param>
                /// <param name="waitTime">等待超时时间(秒)</param>
                /// <returns>事务状态</returns>
                TranStatus Complate(Action<IRpcTran> checkTran, int waitTime = 10);
                /// <summary>
                /// 回滚事务
                /// </summary>
                void Rollback();
        }
}