using System;

namespace WeDonekRpc.Modular
{
    public interface IRpcEventModule : IDisposable
    {
        string Module { get; }
        void Init ();
    }
}