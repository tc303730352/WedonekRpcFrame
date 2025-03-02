using System.Collections.Concurrent;
using System.Collections.Generic;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using WeDonekRpc.Modular.SysEvent.Model;
using WeDonekRpc.ModularModel.SysEvent.Model;

namespace WeDonekRpc.Modular.SysEvent.Module
{
    internal class NoServerErrorModule : IRpcEventModule
    {
        private class _Cache
        {
            public int time;
            public int Interval;
            public long eventId;
        }
        private readonly IRpcService _RpcService;
        private readonly IRpcEventLogService _Service;
        private NoServerErrorEvent _Def;
        public string Module => "NoServerError";
        private readonly ConcurrentDictionary<string, _Cache> _NoServerCahce = new ConcurrentDictionary<string, _Cache>();
        public NoServerErrorModule ( ServiceSysEvent[] obj, IRpcEventLogService service, IRpcService rpcService )
        {
            this._RpcService = rpcService;
            this._Service = service;
            NoServerErrorEvent[] evs = obj.ConvertAll(a => new NoServerErrorEvent(a));
            evs.ForEach(c =>
            {
                if ( !c.IsLimit )
                {
                    this._Def = c;
                }
                else
                {
                    c.SystemType.ForEach(a =>
                    {
                        if ( !this._NoServerCahce.ContainsKey(a) )
                        {
                            _ = this._NoServerCahce.TryAdd(a, new _Cache
                            {
                                time = 0,
                                Interval = c.Interval,
                                eventId = c.EventId
                            });
                        }
                    });
                }
            });
        }
        public void Dispose ()
        {
            this._RpcService.NoServerEvent -= this._RpcService_NoServerEvent;
        }

        private void _RpcService_NoServerEvent ( IRemoteConfig config, string sysType, object model )
        {
            if ( !this._NoServerCahce.TryGetValue(sysType, out _Cache cache) )
            {
                cache = this._Def == null ? new _Cache
                {
                    eventId = 0
                } : new _Cache
                {
                    eventId = this._Def.EventId,
                    Interval = this._Def.Interval,
                    time = HeartbeatTimeHelper.HeartbeatTime + this._Def.Interval
                };
                _ = this._NoServerCahce.TryAdd(sysType, cache);
            }
            else if ( cache.eventId == 0 || cache.time >= HeartbeatTimeHelper.HeartbeatTime )
            {
                return;
            }
            else
            {
                cache.time = HeartbeatTimeHelper.HeartbeatTime + cache.Interval;
            }
            this._WriteLog(config, sysType, model, cache.eventId);
        }

        private void _WriteLog ( IRemoteConfig config, string sysType, object model, long eventId )
        {
            Dictionary<string, string> args = new Dictionary<string, string>()
            {
                  { "systemtype", sysType },
                  { "dictate", config.SysDictate },
                   {"verNum",RpcClient.CurrentSource.VerNum.ToString() },
                   {"rpcmerid",config.RpcMerId.HasValue?config.RpcMerId.Value.ToString():string.Empty },
                   {"transmit",config.Transmit },
             };
            if ( config.TransmitType != TransmitType.close )
            {
                args.Add("identitycolumn", config.IdentityColumn ?? string.Empty);
                object val = config.GetIdentityVal(model);
                if ( val != null )
                {
                    args.Add("identity", val.ToString());
                    if ( config.TransmitType == TransmitType.ZoneIndex )
                    {
                        char[] chars = val.ToString().ToCharArray();
                        if ( !config.ZIndexBit.IsNull() )
                        {
                            args.Add("zoneindex", config.ZIndexBit[0] + "," + config.ZIndexBit[1]);
                            args.Add("zone", Tools.GetZoneIndex(chars, config.ZIndexBit[0], config.ZIndexBit[1]).ToString());
                        }
                        else
                        {
                            args.Add("zone", Tools.GetZoneIndex(chars, 0, chars.Length - 1).ToString());
                        }
                    }
                }
                else
                {
                    args.Add("identity", string.Empty);
                }
            }
            this._Service.AddLog(eventId, args);
        }
        public void Init ()
        {
            this._RpcService.NoServerEvent += this._RpcService_NoServerEvent;
        }
    }
}
