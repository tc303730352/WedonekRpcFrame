using System;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.ModularModel.SysEvent.Model;

namespace WeDonekRpc.Modular
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    public interface IRpcEventService
    {
        ServiceSysEvent[] GetSysEvents ( string module );

        void RefreshEvent ( Action<string> action );
    }
}