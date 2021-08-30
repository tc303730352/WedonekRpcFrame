using System;
using System.Diagnostics;

using RpcClient.Config;
using RpcClient.RpcApi;

using RpcModel;
using RpcModel.Server;

using RpcHelper.TaskTools;

namespace RpcClient.Collect
{
        internal class RpcStateCollect
        {
                private static readonly Process _Process = null;
                private static volatile bool _IsInit = false;
                public static long RpcMerId
                {
                        get;
                        private set;
                }
                public static RpcConfig OAuthConfig { get; private set; }
                public static RpcServerConfig ServerConfig { get; private set; }
                public static MsgSource LocalSource { get; private set; }
                public static long ServerId { get; private set; }
                public static bool IsInit => _IsInit;

                static RpcStateCollect()
                {
                        _Process = Process.GetCurrentProcess();
                        TaskManage.AddTask(new TaskHelper("同步服务状态!", new TimeSpan(0, 1, 0), new Action(_SyncSysState)));
                }

                private static void _SyncSysState()
                {
                        if (_IsInit)
                        {
                                ServiceHeartbeat obj = new ServiceHeartbeat
                                {
                                        ServerId = RpcStateCollect.ServerId,
                                        RunState = _GetRunState(),
                                        RemoteState = RemoteServerCollect.GetRemoteState()
                                };
                                RpcServiceApi.SendHeartbeat(obj);
                        }
                }

                public static bool InitServer(out string error)
                {
                        if (_IsInit)
                        {
                                error = null;
                                return true;
                        }
                        else if (!RpcTokenCollect.GetAccessToken(out RpcToken token, out error))
                        {
                                return false;
                        }
                        else if (!RpcServiceApi.ServerLogin(token, new RemoteServerLogin
                        {
                                SystemType = WebConfig.RpcSystemType,
                                ServerMac = WebConfig.LocalMac,
                                ServerIndex = WebConfig.RpcServerIndex,
                                Process = new ProcessDatum
                                {
                                        Pid = _Process.Id,
                                        PName = _Process.ProcessName,
                                        StartTime = _Process.StartTime,
                                        WorkMemory = _Process.WorkingSet64
                                }
                        }, out RpcConfig config, out RpcServerConfig serverConfig, out error))
                        {
                                return false;
                        }
                        else
                        {
                                _IsInit = true;
                                RpcMerId = token.RpcMerId;
                                ServerId = serverConfig.ServerId;
                                OAuthConfig = config;
                                ServerConfig = serverConfig;
                                LocalSource = new MsgSource
                                {
                                        RegionId = serverConfig.RegionId,
                                        SourceGroupId = serverConfig.GroupId,
                                        GroupTypeVal = serverConfig.GroupTypeVal,
                                        SourceServerId = serverConfig.ServerId,
                                        SystemType = Config.WebConfig.RpcSystemType,
                                        SystemTypeId = serverConfig.SystemType,
                                        RpcMerId = token.RpcMerId
                                };
                                return true;
                        }
                }

                private static RunState _GetRunState()
                {
                        _Process.Refresh();
                        return new RunState
                        {
                                ConNum = SocketTcpServer.SocketTcpServer.GetSocketClientNum(SocketTcpServer.SocketTcpServer.DefaultServerId),
                                Memory = _Process.WorkingSet64,
                                CpuRunTime = (int)_Process.TotalProcessorTime.TotalMilliseconds
                        };
                }

                public static void Close()
                {
                        _Process.Dispose();
                }

        }
}
