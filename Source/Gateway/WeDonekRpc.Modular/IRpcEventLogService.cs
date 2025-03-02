using System.Collections.Generic;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Modular.SysEvent.Model;

namespace WeDonekRpc.Modular
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    public interface IRpcEventLogService
    {
        void AddLog ( BasicEvent ev, Dictionary<string, string> args );
        void AddLog ( long evId, Dictionary<string, string> args );
    }
}