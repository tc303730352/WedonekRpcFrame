using System;

namespace WeDonekRpc.Client.Interface
{
    [Attr.ClassLifetimeAttr(Attr.ClassLifetimeType.SingleInstance)]
    public interface IRpcTwoPcTran
    {
        IDisposable BeginTran (ICurTran cur);
        void Commit (ICurTran cur);
        void Rollback (ICurTran cur);
    }
}