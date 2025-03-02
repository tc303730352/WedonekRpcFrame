using System;
using System.Runtime.InteropServices;
using System.Threading;
using WeDonekRpc.Client.Config;
using WeDonekRpc.Client.Helper;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Client.RpcApi;
using WeDonekRpc.Client.Server;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.Model;
using WeDonekRpc.Model.Server;

namespace WeDonekRpc.Client.Collect
{
    internal class RpcStateCollect
    {
        private static volatile bool _IsInit = false;

        public static long RpcMerId
        {
            get;
            private set;
        }
        public static RpcConfig OAuthConfig { get; private set; }
        public static RpcServerConfig ServerConfig { get; private set; }
        public static MsgSource LocalSource { get; private set; }
        public static LocalRpcConfig LocalConfig { get; private set; }
        public static long ServerId { get; private set; }
        public static bool IsInit => _IsInit;
        private static readonly Timer _SyncTimer;
        static RpcStateCollect ()
        {
            _SyncTimer = new Timer(new TimerCallback(_SyncSysState), null, 30000, 30000);
        }

        private static void _SyncSysState (object state)
        {
            if (_IsInit)
            {
                try
                {
                    ServiceHeartbeat obj = new ServiceHeartbeat
                    {
                        ServerId = RpcStateCollect.ServerId,
                        RunState = _GetRunState(),
                        RemoteState = RemoteServerCollect.GetRemoteState(),
                        VerNum = LocalSource.VerNum
                    };
                    int res = RpcServiceApi.SendHeartbeat(obj);
                    if (res != -1 && res != LocalSource.VerNum)
                    {
                        SetVerNum(res);
                    }
                }
                catch (Exception ex)
                {
                    new ErrorException(ex).Save("def");
                }
            }
        }

        public static bool InitServer (out string error)
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
            else
            {
                RpcBasicConfig basicConfig = WebConfig.BasicConfig;
                RemoteServerLogin login = new RemoteServerLogin
                {
                    SystemType = basicConfig.RpcSystemType,
                    ServerMac = WebConfig.Environment.Mac,
                    ServerIndex = basicConfig.RpcServerIndex,
                    ContainerType = basicConfig.ContainerType,
                    Process = new ProcessDatum
                    {
                        Pid = CurProcessHelper.ProcessId,
                        PName = CurProcessHelper.ProcessName,
                        MachineName = CurProcessHelper.MachineName,
                        StartTime = CurProcessHelper.StartTime,
                        WorkMemory = CurProcessHelper.WorkingSet64,
                        Framework = RuntimeInformation.FrameworkDescription,
                        OSArchitecture = RuntimeInformation.OSArchitecture,
                        OSDescription = RuntimeInformation.OSDescription,
                        ProcessArchitecture = RuntimeInformation.ProcessArchitecture,
                        RuntimeIdentifier = RuntimeInformation.RuntimeIdentifier,
                        RunIsAdmin = WebConfig.Environment.RunIsAdmin,
                        RunUserGroups = Tools.GetRunUserGroups(),
                        RunUserIdentity = WebConfig.Environment.RunUserIdentity,
                        IsLittleEndian = BitConverter.IsLittleEndian,
                        ProcessorCount = CurProcessHelper.ProcessorCount,
                        OSType = Tools.GetOSType(),
                        SystemStartTime = DateTime.Now.AddMilliseconds(-Environment.TickCount64)
                    }
                };
                if (basicConfig.ContainerType != ContainerType.无)
                {
                    int? port = ContainerHelper.GetInsidePort(basicConfig.InsidePort);
                    login.Container = new ContainerInfo
                    {
                        InsidePort = port,
                        HostPort = basicConfig.HostPort.HasValue ? basicConfig.HostPort : port,
                        ContainerId = ContainerHelper.ContainerId(),
                        HostIp = ContainerHelper.GetPhysicalIp(),
                        LocalIp = ContainerHelper.GetLoaclIp(),
                    };
                }
                if (!DataValidateHepler.ValidateData(login, out error))
                {
                    return false;
                }
                else if (!RpcServiceApi.ServerLogin(token, login, out RpcConfig config, out RpcServerConfig serverConfig, out error))
                {
                    return false;
                }
                else
                {
                    RpcMerId = token.RpcMerId;
                    ServerId = serverConfig.ServerId;
                    OAuthConfig = config;
                    ServerConfig = serverConfig;
                    LocalConfig = new LocalRpcConfig
                    {
                        RegionId = serverConfig.RegionId,
                        ContGroup = serverConfig.ContainerGroup,
                        SysGroup = serverConfig.GroupTypeVal,
                        SysGroupId = ServerConfig.GroupId,
                        SystemTypeId = ServerConfig.SystemType,
                        ServerId = serverConfig.ServerId,
                        SystemType = basicConfig.RpcSystemType,
                        RpcMerId = token.RpcMerId
                    };
                    LocalSource = LocalConfig.ConvertMap<LocalRpcConfig, MsgSource>();
                    _IsInit = true;
                    return true;
                }
            }
        }
        private static ThreadPoolState _GetThreadPoolState ()
        {
            ThreadPool.GetMaxThreads(out int maxWorker, out int maxCompletion);
            ThreadPool.GetMinThreads(out int minWorker, out int minCompletion);
            ThreadPool.GetAvailableThreads(out int availWorker, out int availCompletion);
            return new ThreadPoolState
            {
                ThreadCount = ThreadPool.ThreadCount,
                CompletedWorkItemCount = ThreadPool.CompletedWorkItemCount,
                PendingWorkItemCount = ThreadPool.PendingWorkItemCount,
                MaxWorker = maxWorker,
                MinWorker = minWorker,
                MaxCompletionPort = maxCompletion,
                MinCompletionPort = minCompletion,
                AvailableWorker = availWorker,
                AvailableCompletionPort = availCompletion
            };
        }
        private static RunState _GetRunState ()
        {
            return new RunState
            {
                LockContentionCount = Monitor.LockContentionCount,
                ThreadPool = _GetThreadPoolState(),
                ConNum = TcpServer.TcpServer.GetSocketClientNum(TcpServer.TcpServer.DefaultServerId),
                WorkMemory = CurProcessHelper.WorkingSet64,
                CpuRate = CurProcessHelper.CpuRate,
                CpuRunTime = CurProcessHelper.CpuTime,
                GCBody = GCHelper.GetGC(),
                ThreadNum = CurProcessHelper.ThreadNum,
                TimerNum = Timer.ActiveCount
            };
        }

        public static void Close ()
        {
            _SyncTimer.Dispose();
            CurProcessHelper.Dispose();
        }

        internal static void SetVerNum (int verNum)
        {
            if (LocalSource.VerNum != verNum)
            {
                ServerConfig.VerNum = verNum;
                LocalSource.VerNum = verNum;
            }
        }
    }
}
