using System;
using System.Collections.Generic;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Interface;
using WeDonekRpc.Modular.SysEvent.Model;
using WeDonekRpc.ModularModel.SysEvent;
using WeDonekRpc.ModularModel.SysEvent.Model;

namespace WeDonekRpc.Modular.SysEvent
{
    internal class RpcEventLogService : IRpcEventLogService
    {
        private static readonly IDelayDataSave<SysEventLog> _Logs;
        static RpcEventLogService ()
        {
            _Logs = new DelayDataSave<SysEventLog>(_SaveLogs, 2, 10);
        }
        public static void Init ( IRpcService service )
        {
            service.Closing += Service_Closing; ;
        }

        private static void Service_Closing ()
        {
            _Logs.Dispose();
        }

        private static void _SaveLogs ( ref SysEventLog[] datas )
        {
            new AddSysEventLog
            {
                Logs = datas
            }.Send();
        }
        public void AddLog ( BasicEvent ev, Dictionary<string, string> args )
        {
            _Logs.AddData(new SysEventLog
            {
                CreateTime = DateTime.Now,
                EventId = ev.EventId,
                Attrs = args
            });
        }
        public void AddLog ( long evId, Dictionary<string, string> args )
        {
            _Logs.AddData(new SysEventLog
            {
                CreateTime = DateTime.Now,
                EventId = evId,
                Attrs = args
            });
        }
    }
}
