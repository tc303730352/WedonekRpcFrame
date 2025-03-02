using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.ModularModel.SysEvent;
using WeDonekRpc.ModularModel.SysEvent.Model;
using WeDonekRpc.ModularModel.SysEvent.Msg;

namespace WeDonekRpc.Modular.SysEvent
{
    internal class RpcEventService : IRpcEventService
    {
        private static readonly ConcurrentDictionary<string, IRpcEventModule> _Modules = new ConcurrentDictionary<string, IRpcEventModule>();
        private static IRpcService _RpcService;

        private static IRpcEventLogService _LogService;

        private static event Action<string> _RefreshEvent;

        private static void _Refresh ( RefreshEventModule data )
        {
            if ( _Modules.TryRemove(data.Module, out IRpcEventModule module) )
            {
                module.Dispose();
            }
            ServiceSysEvent[] list = new GetEnableSysEvent
            {
                Module = data.Module
            }.Send();
            if ( !list.IsNull() )
            {
                _AddModule(data.Module, list);
            }
            _RefreshEvent?.Invoke(data.Module);
        }
        public ServiceSysEvent[] GetSysEvents ( string module )
        {
            return new GetEnableSysEvent
            {
                Module = module
            }.Send();
        }


        public static void Init ( IRpcService service )
        {
            _LogService = RpcClient.Ioc.Resolve<IRpcEventLogService>();
            _RpcService = service;
            RpcEventLogService.Init(service);
            RpcClient.Route.AddRoute<RefreshEventModule>(_Refresh);
            service.Closing += Service_Closing;
            service.StartUpComplating += Service_BeginIniting;
        }
        private static void _AddModule ( string name, ServiceSysEvent[] evs )
        {
            IRpcEventModule module;
            if ( name == "RpcSendBehavior" )
            {
                module = new Module.RpcSendBehaviorModule(evs, _LogService, _RpcService);
            }
            else if ( name == "CpuBehavior" )
            {
                module = new Module.CpuBehaviorModule(evs, _LogService);
            }
            else if ( name == "MemoryOccupation" )
            {
                module = new Module.MemoryOccupationModule(evs, _LogService);
            }
            else if ( name == "RpcReplyBehavior" )
            {
                module = new Module.RpcReplyBehaviorModule(evs, _LogService, _RpcService);
            }
            else if ( name == "RemoteStateChange" )
            {
                module = new Module.RemoteStateChangeModule(evs, _LogService, _RpcService);
            }
            else if ( name == "NoServerError" )
            {
                module = new Module.NoServerErrorModule(evs, _LogService, _RpcService);
            }
            else
            {
                return;
            }
            if ( _Modules.TryAdd(name, module) )
            {
                module.Init();
            }
        }
        private static void Service_BeginIniting ()
        {
            ServiceSysEvent[] list = new GetEnableSysEvent().Send();
            if ( list.IsNull() )
            {
                return;
            }
            string[] module = list.Distinct(a => a.Module);
            module.ForEach(a =>
            {
                ServiceSysEvent[] evs = list.FindAll(c => c.Module == a);
                _AddModule(a, evs);
            });
        }

        private static void Service_Closing ()
        {
            if ( _Modules.Count > 0 )
            {
                foreach ( KeyValuePair<string, IRpcEventModule> item in _Modules )
                {
                    item.Value.Dispose();
                }
            }
        }

        public void RefreshEvent ( Action<string> action )
        {
            _RefreshEvent += action;
        }
    }
}
