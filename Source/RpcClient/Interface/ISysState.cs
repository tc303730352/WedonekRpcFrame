using System;

using RpcModel.Server;

namespace RpcClient.Interface
{
        internal interface ISysState : IDisposable
        {
                RunState GetRunState();
        }
}