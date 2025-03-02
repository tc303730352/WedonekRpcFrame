using System;
using WeDonekRpc.ApiGateway.Config;

namespace WeDonekRpc.ApiGateway.Interface
{
    public interface IIpBlackConfig
    {
        bool IsEnable { get; }
        bool IsLocal { get; }
        IpBlackLocal Local { get; }
        IpBlackRemote Remote { get; }

        void AddRefreshEvent (Action<string> action);
    }
}