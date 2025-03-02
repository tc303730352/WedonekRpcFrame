using System;
using WeDonekRpc.Client.Attr;

namespace WeDonekRpc.Modular
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    public interface IRpcShieldConfig
    {
        bool IsEnable { get; }
        bool IsLocal { get; }
        string[] DirectList { get; }

        void AddRefreshEvent (Action<IRpcShieldConfig> action);
    }
}