using System.Text;
using RpcSync.Collect;
using RpcSync.Model;
using RpcSync.Service.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using WeDonekRpc.ModularModel.SysEvent.Model;

namespace RpcSync.Service.Service
{
    internal class SystemEventLogService : ISystemEventLogService
    {
        private readonly ISystemEventLogCollect _SysEventLog;
        private readonly IServerEventCollect _ServiceEvent;
        private readonly ISystemTypeCollect _SystemType;
        private readonly IRemoteServerCollect _RemoteServer;
        public SystemEventLogService (ISystemEventLogCollect sysEventLog,
            ISystemTypeCollect systemType,
            IRemoteServerCollect remoteServer,
            IServerEventCollect serverEvent)
        {
            this._RemoteServer = remoteServer;
            this._SystemType = systemType;
            this._SysEventLog = sysEventLog;
            this._ServiceEvent = serverEvent;
        }

        private string _FormatEventShow (ServiceEventDatum module, SysEventLog log, MsgSource source)
        {
            if (module.MsgTemplate.IsNull())
            {
                return string.Empty;
            }
            else if (module.MsgTemplate.IndexOfAny(new char[] { '{', '}' }) == -1)
            {
                return module.MsgTemplate;
            }
            int index = 0;
            StringBuilder str = new StringBuilder(module.MsgTemplate);
            string template = module.MsgTemplate;
            List<string> remotes = [];
            List<string> sources = [];
            do
            {
                index = template.IndexOf('{', index);
                if (index == -1)
                {
                    break;
                }
                int end = template.IndexOf('}', index);
                if (end == -1)
                {
                    break;
                }
                string type = template.Substring(index, end - index + 1);
                index = end;
                string show = string.Empty;
                if (type.Length > 0)
                {
                    if (type.StartsWith("{remote_"))
                    {
                        remotes.Add(type);
                        continue;
                    }
                    if (type.StartsWith("{server_"))
                    {
                        sources.Add(type);
                        continue;
                    }
                    switch (type.ToLower())
                    {
                        case "{eventlevel}":
                            show = module.EventLevel.ToString();
                            break;
                        case "{systemtypename}":
                            if (log.Attrs.TryGetValue("systemtype", out string typeVal))
                            {
                                show = this._SystemType.GetName(typeVal);
                            }
                            else
                            {
                                show = string.Empty;
                            }
                            break;
                        case "{eventtype}":
                            show = module.EventType.ToString();
                            break;
                        case "{eventname}":
                            show = module.EventName;
                            break;
                        case "{threshold}":
                            if (log.Attrs.TryGetValue("threshold", out string threshold))
                            {
                                show = threshold;
                            }
                            break;
                        case "{duration}":
                            if (log.Attrs.TryGetValue("duration", out string duration))
                            {
                                show = Tools.FormatTimeMilli(long.Parse(duration));
                            }
                            else
                            {
                                show = "未知";
                            }
                            break;
                        case "{iserror}":
                            if (log.Attrs.TryGetValue("iserror", out string isError))
                            {
                                show = isError == "True" ? "是" : "否";
                            }
                            else
                            {
                                show = "未知";
                            }
                            break;
                        default:
                            string t = type.Substring(1, type.Length - 2);
                            show = log.Attrs.GetValueOrDefault(t, string.Empty);
                            break;
                    }
                }
                _ = str.Replace(type, show);
            } while (true);
            if (remotes.Count > 0)
            {
                long remoteId = long.Parse(log.Attrs.GetValueOrDefault("remote_id", "0"));
                this._InitServer(remoteId, remotes, str);
            }
            if (sources.Count > 0)
            {
                this._InitServer(source.ServerId, sources, str);
            }
            return str.ToString();
        }
        private void _InitServer (long serverId, List<string> seats, StringBuilder show)
        {
            if (serverId == 0)
            {
                seats.ForEach(c =>
                {
                    _ = show.Replace(c, string.Empty);
                });
            }
            else
            {
                RemoteServerConfig remote = this._RemoteServer.GetServer(serverId);
                seats.ForEach(c =>
                {
                    int index = c.IndexOf('_') + 1;
                    string type = c.Substring(index, c.Length - index - 1);
                    string str = string.Empty;
                    switch (type)
                    {
                        case "name":
                            str = remote.ServerName;
                            break;
                        case "systemtype":
                            str = remote.SystemType;
                            break;
                        case "systemtypename":
                            str = this._SystemType.GetName(remote.SystemType);
                            break;
                        case "group":
                            str = remote.GroupName;
                            break;
                        case "ip":
                            str = remote.ConIp ?? remote.ServerIp;
                            break;
                        case "port":
                            str = remote.ServerPort.ToString();
                            break;
                        case "ip:port":
                            str = ( remote.ConIp ?? remote.ServerIp ) + ":" + remote.ServerPort.ToString();
                            break;
                    }
                    _ = show.Replace(c, str);
                });
            }
        }
        public void Add (SysEventLog[] logs, MsgSource source)
        {
            ServiceEventDatum[] modules = this._ServiceEvent.Gets(logs.Distinct(c => c.EventId));
            SysEventAddLog[] adds = logs.Convert<SysEventLog, SysEventAddLog>(a =>
            {
                ServiceEventDatum module = modules.Find(c => c.Id == a.EventId);
                if (module == null)
                {
                    return null;
                }
                return new SysEventAddLog
                {
                    AddTime = a.CreateTime,
                    EventAttr = a.Attrs.ToJson(),
                    EventLevel = module.EventLevel,
                    EventName = module.EventName,
                    ServerId = source.ServerId,
                    SystemTypeId = source.SystemTypeId,
                    EventSourceId = module.SysEventId,
                    EventType = module.EventType,
                    RpcMerId = source.RpcMerId,
                    RegionId = source.RegionId,
                    EventShow = this._FormatEventShow(module, a, source)
                };
            });

            if (adds.Length > 0)
            {
                this._SysEventLog.Adds(adds);
            }
        }

    }
}
