using System;

namespace WeDonekRpc.ApiGateway.Interface
{
    public interface IIpLimitConfig
    {
        bool IsEnable { get; }
        bool IsLocal { get; }
        int LimitNum { get; }
        int LimitTime { get; }
        void AddRefreshEvent (Action<string> action);
    }
}