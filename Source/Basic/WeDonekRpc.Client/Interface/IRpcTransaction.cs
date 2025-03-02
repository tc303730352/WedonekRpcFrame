using System;

namespace WeDonekRpc.Client.Interface
{
    public interface IRpcTransaction : IDisposable
    {
        /// <summary>
        /// 设置事务扩展
        /// </summary>
        /// <param name="extend"></param>
        void SetExtend(string extend);
        /// <summary>
        /// 完成事务
        /// </summary>
        void Complate();
    }
}