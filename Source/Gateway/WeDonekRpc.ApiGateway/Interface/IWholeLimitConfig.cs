using System;

namespace WeDonekRpc.ApiGateway.Interface
{
    public interface IWholeLimitConfig : ILimitConfig
    {

        void AddRefreshEvent (Action<string> action);
    }
}