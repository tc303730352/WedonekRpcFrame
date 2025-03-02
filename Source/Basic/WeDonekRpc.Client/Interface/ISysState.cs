using System;

using WeDonekRpc.Model.Server;

namespace WeDonekRpc.Client.Interface
{
        internal interface ISysState : IDisposable
        {
                RunState GetRunState();
        }
}