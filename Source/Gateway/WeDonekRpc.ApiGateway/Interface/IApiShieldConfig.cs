using System;

namespace WeDonekRpc.ApiGateway.Interface
{
    public interface IApiShieldConfig
    {
        bool IsEnable { get; }
        bool IsLocal { get; }
        string[] ShieIdPath { get; }

        void AddRefreshEvent (Action<string> action);
    }
}