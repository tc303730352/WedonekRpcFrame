using System;
using System.Collections.Generic;

namespace WeDonekRpc.ModularModel.SysEvent.Model
{
    public class SysEventLog
    {
        public long EventId
        {
            get;
            set;
        }
        public Dictionary<string, string> Attrs
        {
            get;
            set;
        }
        public DateTime CreateTime
        {
            get;
            set;
        }
    }
}
