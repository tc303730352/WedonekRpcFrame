using System;

namespace RpcHelper
{
        public interface IDataQueueHelper<T> : IDisposable
        {
                void AddQueue(T data);

                void AddQueue(T[] list);
                void SetTimer(int dueTime, int period);
        }
}