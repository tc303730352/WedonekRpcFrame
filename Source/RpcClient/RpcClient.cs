using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

using RpcCacheClient;
using RpcCacheClient.Config;
using RpcCacheClient.Interface;

using RpcClient.Broadcast;
using RpcClient.Collect;
using RpcClient.Config;
using RpcClient.Helper;
using RpcClient.Interface;
using RpcClient.Model;
using RpcClient.RpcSysEvent;

using RpcModel;

using RpcHelper;

namespace RpcClient
{
        /// <summary>
        /// Rpc 客户端
        /// </summary>
        public class RpcClient
        {
                public static event Action InitComplate;
                #region 私有变量

                private static readonly SyncLock _Lock = new SyncLock();

                //缓存模块
                private static ICacheController _Cache = null;
                //本地事件
                private static readonly ILocalEventCollect _LocalEvent = new LocalEventCollect();
                //错误模块
                private static readonly IErrorCollect _Error = new ErrorCollect();
                //配置模块
                private static readonly ISysConfig _Config = new Collect.SysConfigCollect();
                //路由模块
                private static readonly ITcpRouteCollect _Route = new TcpRouteCollect();

                //订阅模块
                private static readonly IRpcSubscribeCollect _Subscribe = new RpcSubscribeCollect();

                private static readonly IRpcControlCollect _RpcServer = new RpcControlCollect();
                /// <summary>
                /// 事务模块
                /// </summary>
                private static readonly IRpcTranCollect _RpcTran = new RpcTranCollect();

                #endregion

                static RpcClient()
                {
                        AppDomain.CurrentDomain.AssemblyLoad += CurrentDomain_AssemblyLoad;
                        Unity.RegisterEvent += Unity_RegisterEvent;
                        _InitUnity();
                        Unity.Load("RpcClient");
                        RemoteSysEvent.AddEvent<ResetServiceState>("Rpc_ResetState", _ResetState);
                        Unity.RegisterInstance<IRpcService>(RpcService.Service);
                }

                private static void _InitUnity()
                {
                        Unity.RegisterInstance(RpcClient.LocalEvent);
                        Unity.RegisterInstance(RpcClient.Unity);
                        Unity.RegisterInstance(RpcClient._Route);
                        Unity.RegisterInstance(RpcClient._Config);
                        Unity.RegisterInstance(RpcClient._Subscribe);
                        Unity.RegisterInstance(RpcClient._RpcTran);
                }

                private static void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
                {
                        IRpcModular modular = Tools.CreateObject<IRpcModular>(args.LoadedAssembly);
                        if (modular != null)
                        {
                                LoadModular(modular);
                        }
                }

                private static void Unity_RegisterEvent(IocBody body)
                {
                        if (body.Form == ConfigDic.IExtendService)
                        {
                                IExtendService service = Unity.Resolve<IExtendService>(body.Name);
                                service.Load(RpcService.Service);
                        }
                }

                /// <summary>
                /// 是否正在关闭
                /// </summary>
                internal static bool IsClosing = false;


                #region 公有属性

                public static string ClientVer => WebConfig.ApiVerNum;
                public static string AppSecret => WebConfig.AppSecret;
                /// <summary>
                /// Rpc事件
                /// </summary>
                public static IRpcEvent RpcEvent
                {
                        get;
                        set;
                } = new RpcEvent();
                /// <summary>
                /// 本地事件总线
                /// </summary>
                public static ILocalEventCollect LocalEvent => _LocalEvent;
                /// <summary>
                /// 容器
                /// </summary>
                public static IUnityCollect Unity
                {
                        get;
                } = new UnityCollect();


                /// <summary>
                /// 缓存模块
                /// </summary>
                public static ICacheController Cache => _Cache;
                /// <summary>
                /// 系统类型
                /// </summary>
                public static string SystemTypeVal => WebConfig.RpcSystemType;

                /// <summary>
                /// 是否已初始化
                /// </summary>
                public static bool IsInit => RpcStateCollect.IsInit;
                /// <summary>
                /// 服务节点Id
                /// </summary>
                public static long ServerId => RpcStateCollect.ServerId;
                /// <summary>
                /// 当前消息源
                /// </summary>
                public static MsgSource CurrentSource => RpcStateCollect.LocalSource;
                /// <summary>
                /// 路由模块
                /// </summary>
                public static ITcpRouteCollect Route => _Route;
                /// <summary>
                /// 配置管理模块
                /// </summary>
                public static ISysConfig Config => _Config;
                /// <summary>
                /// 订阅模块
                /// </summary>
                public static IRpcSubscribeCollect Subscribe => _Subscribe;
                /// <summary>
                /// 错误模块
                /// </summary>
                public static IErrorCollect Error => _Error;

                /// <summary>
                /// 中控模块
                /// </summary>
                internal static IRpcControlCollect RpcServer => _RpcServer;

                /// <summary>
                /// 事务模块
                /// </summary>
                public static IRpcTranCollect RpcTran => _RpcTran;

                #endregion


                #region 初始化

                private bool _InitRpc()
                {
                        if (_Lock.GetLock())
                        {
                                if (RpcStateCollect.InitServer(out string error))
                                {
                                        _Lock.Exit();
                                        return true;
                                }
                                else
                                {
                                        _Lock.Reset();
                                        string show = string.Format("mac:{0}\r\nappId:{1}\r\nSystemType:{2}\r\nSecret:{3}", WebConfig.LocalMac, WebConfig.AppId, WebConfig.RpcSystemType, WebConfig.AppSecret);
                                        RpcLogSystem.AddFatalError("初始化OAuth失败!", show, error);
                                        return false;
                                }
                        }
                        else
                        {
                                return RpcStateCollect.IsInit;
                        }
                }
                private bool _RetryInit(int retry)
                {
                        Thread.Sleep(retry * 1000 % 5000);
                        if (this._InitRpc())
                        {
                                return true;
                        }
                        else
                        {
                                ++retry;
                                return retry <= 5 && this._RetryInit(retry);
                        }
                }
                private bool _InitRpc(out string error)
                {
                        if (!this._Init())
                        {
                                error = "rpc.oauth.init.error";
                                return false;
                        }
                        else
                        {
                                return SysConfigCollect.LoadSysConfig(out error);
                        }
                }
                /// <summary>
                /// 初始化缓存模块
                /// </summary>
                private void _InitCache()
                {
                        CacheConfig config = WebConfig.GetCacheConfig();
                        RpcCacheService.Init(config, WebConfig.RpcConfig.CacheType);
                        _Cache = RpcCacheService.GetCache(WebConfig.RpcConfig.CacheType);
                        Unity.RegisterInstance(RpcClient._Cache);
                }
                private bool _Init()
                {
                        return this._InitRpc() || this._RetryInit(1);
                }

                /// <summary>
                /// 初始化模块
                /// </summary>
                private void _InitModular()
                {
                        this._InitCache();
                        TrackCollect.Init();
                        SysLogCollect.Init();
                        BroadcastCollect.Init();
                        this._InitSocket();
                        this._InitComplate();
                }


                private void _InitSocket()
                {
                        TrackCollect.EnableChange += (e) =>
                        {
                                SocketTcpServer.Config.SocketConfig.DefaultAllot = e.IsEnable ? new Msg.TrackTcpMsg() : new Msg.TcpMsg();
                        };
                        SocketTcpServer.Config.SocketConfig.DefaultAllot = TrackCollect.IsEnable ? new Msg.TrackTcpMsg() : new Msg.TcpMsg();
                        SocketTcpServer.Config.SocketConfig.DefaultServerPort = RpcStateCollect.ServerConfig.ServerPort;
                        SocketTcpServer.Config.SocketConfig.ServerKey = RpcStateCollect.ServerConfig.PublicKey;
                        SocketTcpServer.Config.SocketConfig.SocketEvent = new TcpEvent();
                        SocketTcpServer.SocketTcpServer.Init();
                }
                #endregion

                /// <summary>
                /// 检查授权Ip
                /// </summary>
                /// <param name="clientIp"></param>
                /// <returns></returns>
                internal static bool CheckAccreditIp(string clientIp)
                {
                        return clientIp == ConfigDic.LocalIp || RpcStateCollect.OAuthConfig.CheckAccreditIp(clientIp);
                }
                /// <summary>
                /// 刷新服务节点状态
                /// </summary>
                /// <param name="state">服务状态</param>
                /// <returns>远程回复消息</returns>
                private static TcpRemoteReply _ResetState(ResetServiceState state)
                {
                        RpcEvent.ServiceState(state.ServiceState);
                        if (state.ServiceState != RpcServiceState.停用)
                        {
                                return null;
                        }
                        Close();
                        return null;
                }
                /// <summary>
                /// 关闭服务
                /// </summary>
                private static void _Close()
                {
                        SocketTcpServer.SocketTcpServer.CloseServer();//关闭TCP服务
                        RpcStateCollect.Close();//释放进程信息
                        RpcCacheService.Close();//释放缓存
                        _Subscribe.Dispose();//释放订阅队列
                        _Route.Dispose();//释放事件
                        RpcEvent.ServerClose();//触发关闭事件
                        SocketTcpClient.TcpClient.CloseSocket();//释放TCP客户端链接
                        RpcLogSystem.AddLog("服务已关闭!");
                        LogSystem.CloseLog();//关闭日志
                }

                #region 公有方法
                /// <summary>
                /// 关闭服务
                /// </summary>
                public static void Close()
                {
                        Close(WebConfig.RpcConfig.CloseDelayTime);
                }
                /// <summary>
                /// 关闭服务
                /// </summary>
                /// <param name="time">延迟释放时间(秒)</param>
                public static void Close(int time)
                {
                        if (RpcClient.IsClosing)
                        {
                                return;
                        }
                        RpcClient.IsClosing = true;//设为关闭状态后续收到的请求返回固定错误码
                        RpcHelper.TaskTools.TaskManage.StopTask();//停止定时任务
                        RpcQueueCollect.Dispose();//关闭消息队列
                        Thread.Sleep(time * 1000);//延迟等待正在处理的任务处理完成
                        _Close();//关闭服务
                }
                /// <summary>
                /// 触发初始化完成事件
                /// </summary>
                private void _InitComplate()
                {
                        if (InitComplate != null)
                        {
                                InitComplate();
                        }
                        RpcService.Service.StartUp();
                }

                /// <summary>
                /// 启动服务
                /// </summary>
                public static void Start()
                {
                        RpcEvent.ServerStarting();
                        RpcClient client = new RpcClient();
                        if (!client._InitRpc(out string error))
                        {
                                throw new Exception(error);
                        }
                        else if (!ServerLimitCollect.Init(out error))
                        {
                                throw new Exception(error);
                        }
                        else
                        {
                                client._InitModular();
                                RpcEvent.ServerStarted();
                                RpcLogSystem.AddLog("服务启动成功!");
                                Process pro = Process.GetCurrentProcess();
                                pro.EnableRaisingEvents = true;
                                pro.Exited += new EventHandler((a, e) =>
                                {
                                        RpcClient.Close();
                                });
                        }
                }
                /// <summary>
                /// 获取授权码
                /// </summary>
                /// <returns></returns>
                public static RpcToken GetAccessToken()
                {
                        if (RpcTokenCollect.GetAccessToken(out RpcToken token, out string error))
                        {
                                return token;
                        }
                        throw new ErrorException(error);
                }
                public static void LoadModular(IRpcModular modular)
                {
                        Type type = modular.GetType();
                        Load(type.Assembly);
                        modular.Init();
                }
                /// <summary>
                /// 注册程序集中可用容器和事件
                /// </summary>
                /// <param name="assemblyName"></param>
                public static void Load(string assemblyName)
                {
                        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                        Assembly obj = assemblies.Find(a => a.GetName().Name == assemblyName);
                        if (obj != null)
                        {
                                Load(obj);
                        }
                }
                /// <summary>
                /// 注册程序集中可用容器和事件
                /// </summary>
                /// <param name="assembly"></param>
                public static void Load(Assembly assembly)
                {
                        Unity.Load(assembly);
                        Unity.Load(assembly, ConfigDic.IExtendService);
                        _LocalEvent.RegLocalEvent(assembly);
                        new RpcRouteHelper().AddRoute(assembly);
                        new RpcSubscribeHelper().AddRoute(assembly);
                }
                #endregion
        }
}
