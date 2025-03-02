using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Config;
using WeDonekRpc.Client.EventBus;
using WeDonekRpc.Client.Identity;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Client.Log;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Client.Server;
using WeDonekRpc.Client.TranService;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Lock;
using WeDonekRpc.Helper.Log;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client
{
    /// <summary>
    /// Rpc 客户端
    /// </summary>
    public class RpcClient
    {
        public static event Action InitComplate;
        #region 私有变量

        private static readonly SyncLock _Lock = new SyncLock();

        /// <summary>
        /// 本地事件
        /// </summary>
        private static readonly ILocalEventCollect _LocalEvent = new LocalEventService();

        /// <summary>
        /// 配置模块
        /// </summary>
        private static readonly ISysConfig _Config = new Collect.SysConfigCollect();
        /// <summary>
        /// 路由模块
        /// </summary>
        private static readonly IRouteService _Route = new RouteService.RouteService();

        /// <summary>
        /// 订阅模块
        /// </summary>
        private static readonly IRpcSubscribeCollect _Subscribe = new RpcSubscribeCollect();

        private static readonly IRpcControlCollect _RpcServer = new RpcControlCollect();
        /// <summary>
        /// 事务模块
        /// </summary>
        private static readonly IRpcTranService _RpcTran = new RpcTranService();

        #endregion

        static RpcClient ()
        {
            NodeEventHandler.Init();
        }

        internal static void SendEvent (ref SendBody send, int retryNum)
        {
            RpcService.Service.SendEvent(ref send, retryNum);
        }
        internal static void SendEnd (ref SendBody send, IRemoteResult result)
        {
            RpcService.Service.SendEnd(ref send, result);
        }
        /// <summary>
        /// 是否正在关闭
        /// </summary>
        internal static bool IsClosing = false;


        #region 公有属性

        public static string ClientVer => WebConfig.ApiVerNum;
        public static string AppSecret => WebConfig.BasicConfig.AppSecret;
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
        public static IIocService Ioc
        {
            get;
        } = new IocService();


        /// <summary>
        /// 系统类型
        /// </summary>
        public static string SystemTypeVal => WebConfig.BasicConfig.RpcSystemType;
        /// <summary>
        /// 系统组别
        /// </summary>
        public static string GroupTypeVal => RpcStateCollect.LocalConfig.SysGroup;
        /// <summary>
        /// 服务名称
        /// </summary>
        public static string ServerName => RpcStateCollect.ServerConfig.Name;

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
        public static IRouteService Route => _Route;
        /// <summary>
        /// 配置管理模块
        /// </summary>
        public static ISysConfig Config => _Config;
        /// <summary>
        /// 订阅模块
        /// </summary>
        public static IRpcSubscribeCollect Subscribe => _Subscribe;


        /// <summary>
        /// 中控模块
        /// </summary>
        internal static IRpcControlCollect RpcServer => _RpcServer;

        /// <summary>
        /// 事务模块
        /// </summary>
        public static IRpcTranService RpcTran => _RpcTran;

        #endregion


        #region 初始化
        /// <summary>
        /// 登陆服务中心
        /// </summary>
        /// <returns></returns>
        private bool _LoginCentralService (out string error)
        {
            if (_Lock.GetLock())
            {
                if (RpcStateCollect.InitServer(out error))
                {
                    _Lock.Exit();
                    return true;
                }
                else
                {
                    _Lock.Reset();
                    string show = string.Format("mac:{0}\r\nappId:{1}\r\nSystemType:{2}\r\nSecret:{3}\r\nerror:{4}",
                        WebConfig.Environment.Mac,
                        WebConfig.BasicConfig.AppId,
                        WebConfig.BasicConfig.RpcSystemType,
                        WebConfig.BasicConfig.AppSecret, error);
                    RpcLogSystem.AddFatalError("服务中心登陆失败!", show, error);
                    return false;
                }
            }
            else
            {
                error = null;
                return RpcStateCollect.IsInit;
            }
        }
        /// <summary>
        /// 登陆重试
        /// </summary>
        /// <param name="retry"></param>
        /// <returns></returns>
        private bool _RetryLogin (int retry, out string error)
        {
            Thread.Sleep(retry * 1000 % 5000);
            try
            {
                if (this._LoginCentralService(out error))
                {
                    return true;
                }
                else if (error == "rpc.server.no.enable")
                {
                    return false;
                }
                retry += 1;
                return retry <= 5 && this._RetryLogin(retry, out error);
            }
            catch (Exception e)
            {
                RpcLogSystem.AddErrorLog("Rpc中控服务登陆异常!", e);
                if (retry <= 5)
                {
                    error = "rpc.server.login.fail";
                    return false;
                }
                return this._RetryLogin(retry, out error);
            }
        }
        private void _LoadConfig (int retryNum)
        {
            try
            {
                SysConfigCollect.LoadSysConfig();
            }
            catch (Exception e)
            {
                if (retryNum > 3)
                {
                    throw new ErrorException(e, LogGrade.Critical);
                }
                this._LoadConfig(++retryNum);
            }
        }
        /// <summary>
        /// 初始化服务节点基础模块
        /// </summary>
        /// <exception cref="ErrorException"></exception>
        private void _InitRpcService ()
        {
            //登陆
            if (this._Login(out string error))
            {
                RpcService.Service.BeginInit();
                //加载基础配置系统模块
                this._LoadConfig(0);
            }
            else if (error == "rpc.server.no.enable")
            {
                RpcLogSystem.AddLog("当前服务未启用,服务将在5秒后自动关闭!");
                Close(5);
            }
            else
            {
                throw new ErrorException(error, LogGrade.Critical);
            }
        }

        /// <summary>
        /// 登陆服务中心
        /// </summary>
        /// <returns></returns>
        private bool _Login (out string error)
        {
            return this._LoginCentralService(out error) || this._RetryLogin(1, out error);
        }

        /// <summary>
        /// 初始化模块
        /// </summary>
        private void _InitModular ()
        {
            RpcService.Service.InitEvent();
            TrackCollect.Init();
            SysLogCollect.Init();
            IdentityService.InitServie();
            BroadcastCollect.Init();
            this._InitComplating();
            this._InitSocket();
            this._InitComplate();
        }

        private void _InitSocket ()
        {
            TrackCollect.EnableChange += (e) =>
            {
                TcpServer.Config.SocketConfig.DefaultAllot = e.IsEnable ? new Msg.TrackTcpMsg() : new Msg.TcpMsg();
            };
            TcpServer.Config.SocketConfig.DefaultAllot = TrackCollect.IsEnable ? new Msg.TrackTcpMsg() : new Msg.TcpMsg();
            TcpServer.Config.SocketConfig.DefaultServerPort = RpcStateCollect.ServerConfig.ServerPort;
            TcpServer.Config.SocketConfig.ServerKey = RpcStateCollect.ServerConfig.PublicKey;
            TcpServer.Config.SocketConfig.SocketEvent = new TcpEvent();
            TcpServer.TcpServer.Init();
        }
        #endregion

        /// <summary>
        /// 检查授权Ip
        /// </summary>
        /// <param name="clientIp"></param>
        /// <returns></returns>
        internal static bool CheckAccreditIp (string clientIp)
        {
            return clientIp == ConfigDic.LocalIp || RpcStateCollect.OAuthConfig.CheckAccreditIp(clientIp);
        }

        /// <summary>
        /// 关闭服务
        /// </summary>
        private static void _Close ()
        {
            TcpServer.TcpServer.CloseServer();//关闭TCP服务
            RpcStateCollect.Close();//释放进程信息
            _Subscribe.Dispose();//释放订阅队列
            _Route.Dispose();//释放事件
            RpcEvent.ServerClose();//触发关闭事件
            TcpClient.TcpClient.CloseSocket();//释放TCP客户端链接
            RpcLogSystem.AddLog("服务已关闭!");
            LogSystem.CloseLog();//关闭日志
        }

        #region 公有方法
        /// <summary>
        /// 关闭服务
        /// </summary>
        public static void Close ()
        {
            Close(WebConfig.RpcConfig.CloseDelayTime);
        }
        /// <summary>
        /// 关闭服务
        /// </summary>
        /// <param name="time">延迟释放时间(秒)</param>
        public static async void Close (int time)
        {
            if (RpcClient.IsClosing)
            {
                return;
            }
            RpcClient.IsClosing = true;//设为关闭状态后续收到的请求返回固定错误码
            RpcService.Service.ServiceClosing();
            RpcQueueCollect.Dispose();//关闭消息队列
            if (time == 0)
            {
                _Close();//关闭服务
            }
            else
            {
                await Task.Delay(time * 1000);//延迟等待正在处理的任务处理完成
                _Close();//关闭服务
            }
        }
        /// <summary>
        /// 触发初始化完成事件
        /// </summary>
        private void _InitComplate ()
        {
            if (InitComplate != null)
            {
                InitComplate();
            }
            RpcService.Service.StartUp();
        }
        private void _InitComplating ()
        {
            RpcService.Service.StartUpIng();
        }


        /// <summary>
        /// 启动服务
        /// </summary>
        public static void Start (Action<RpcInitOption> loadEv = null, Action<IIocService> initEv = null)
        {
            using (RpcInitOption option = new RpcInitOption(RpcEvent))
            {
                option.Init(loadEv, initEv);
            }
            RpcEvent.ServerStarting();
            RpcClient client = new RpcClient();
            client._InitRpcService();
            ServerLimitCollect.Init();
            client._InitModular();
            RpcEvent.ServerStarted();
            EnvironmentCollect.Init();
            Process pro = Process.GetCurrentProcess();
            pro.EnableRaisingEvents = true;
            pro.Exited += new EventHandler((a, e) =>
            {
                RpcClient.Close();
            });
        }
        /// <summary>
        /// 获取授权码
        /// </summary>
        /// <returns></returns>
        public static RpcToken GetAccessToken ()
        {
            if (!RpcTokenCollect.GetAccessToken(out RpcToken token, out string error))
            {
                throw new ErrorException(error);
            }
            return token;
        }


        #endregion
    }
}
