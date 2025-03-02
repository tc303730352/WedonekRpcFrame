using System;
using WeDonekRpc.ApiGateway.Config;

namespace WeDonekRpc.ApiGateway.Interface
{
    public interface INodeLimitConfig : ILimitConfig
    {

        string[] Excludes { get; }

        NodeLimitRange LimitRange { get; }

        string[] Limits { get; }

        void AddRefreshEvent (Action<string> action);
    }
}