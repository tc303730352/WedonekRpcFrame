using System;
using System.IO;
using System.Threading;
using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Limit;
using WeDonekRpc.Client.RpcApi;
using WeDonekRpc.Client.Server;
using WeDonekRpc.Helper;
using WeDonekRpc.IOSendInterface;
using WeDonekRpc.Model;
using WeDonekRpc.Model.Model;
using WeDonekRpc.Model.Server;
namespace WeDonekRpc.Client.Remote
{
    /// <summary>
    /// 远程节点服务
    /// </summary>
    internal class RemoteRootNode : DataSyncClass, IRemoteRootNode
    {

        public RemoteRootNode (long serverId)
        {
            this.ServerId = serverId;
        }

        /// <summary>
        /// 节点是否可用
        /// </summary>
        private volatile bool _IsUsable = true;
        /// <summary>
        /// 远程服务是否可用 0 可用
        /// </summary>
        private int _State = 0;
        /// <summary>
        /// 降级时间
        /// </summary>
        private int _ReduceTime = 0;
        /// <summary>
        /// 刷新时间
        /// </summary>
        private int _RefreshTime = 0;
        /// <summary>
        /// 客户端对象
        /// </summary>
        private ISendClient _Client = null;

        /// <summary>
        /// 执行时长
        /// </summary>
        private long _ExecTime = 0;

        /// <summary>
        /// 发送量
        /// </summary>
        private int _SendNum = 0;
        /// <summary>
        /// 累计的传输级别的错误量
        /// </summary>
        private int _SocketErrorNum = 0;

        /// <summary>
        /// 降级熔断配置
        /// </summary>
        private ReduceInRank _Reduce = null;
        /// <summary>
        /// 计算连续发生链接错误量（用于熔断）
        /// </summary>
        private int _ErrorNum = 0;

        private int _FailSendNum = 0;
        /// <summary>
        /// 节点限制
        /// </summary>
        private IServerLimit _Limit = null;

        /// <summary>
        /// 服务器ID
        /// </summary>
        public long ServerId { get; } = 0;
        /// <summary>
        /// 服务地址
        /// </summary>
        public string ServerIp
        {
            get;
            private set;
        }
        /// <summary>
        /// 端口
        /// </summary>
        public int Port
        {
            get;
            private set;
        }
        /// <summary>
        /// 区域Id
        /// </summary>
        public int RegionId { get; private set; }
        /// <summary>
        /// 最后离线时间
        /// </summary>
        public int OfflineTime { get; private set; }
        /// <summary>
        /// 是否可用
        /// </summary>
        public bool IsUsable => this._IsUsable;
        /// <summary>
        /// 系统类别
        /// </summary>
        public long SystemType { get; private set; } = 0;
        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServerName { get; private set; }

        /// <summary>
        /// 降级结束时间
        /// </summary>
        public int ReduceTime { get => this._ReduceTime; }

        /// <summary>
        /// 节点状态
        /// </summary>
        private RpcServiceState _ServiceState = RpcServiceState.正常;
        /// <summary>
        /// 限流类型
        /// </summary>
        private ServerLimitType _LimitType = ServerLimitType.不启用;
        /// <summary>
        /// 熔断恢复时临时限流数
        /// </summary>
        private int _RecoveryLimit = 0;
        /// <summary>
        /// 熔断恢复时临时限流时间
        /// </summary>
        private int _RecoveryTime = 0;

        private long _RpcMerId;
        protected override void SyncData ()
        {
            if (!RpcTokenCollect.GetAccessToken(out RpcToken token, out string error))
            {
                throw new ErrorException(error);
            }
            else if (!RpcServiceApi.GetRemoteServerData(token, this.ServerId, out ServerConfigInfo config, out error))
            {
                throw new ErrorException(error);
            }
            else
            {
                this._RpcMerId = config.RpcMerId;
                this._RecoveryLimit = config.RecoveryLimit;
                this._RecoveryTime = config.RecoveryTime;
                this.RegionId = config.RegionId;
                this.ServerIp = config.ServerIp;
                this.Port = config.ServerPort;
                this.SystemType = config.SystemType;
                this.ServerName = config.Name;
                if (this._Limit == null)
                {
                    this._InitLimit(config.ClientLimit);
                }
                if (this._Reduce == null)
                {
                    this._Reduce = config.Reduce;
                    if (this._Reduce.IsEnable)
                    {
                        this._RefreshTime = HeartbeatTimeHelper.HeartbeatTime + this._Reduce.RefreshTime;
                    }
                }
                this._InitTcp(config);
                this._InitState(config.ServiceState);
            }
        }
        private void _InitTcp (ServerConfigInfo config)
        {
            //添加建立链接的必要参数
            TcpClient.Config.SocketConfig.AddConArg(config.ServerIp, config.ServerPort, config.PublicKey, new string[]
            {
                RpcClient.ServerId.ToString()
            });
            if (this._Client == null)
            {
                this._Client = new TcpClient.TcpClient(config.ServerIp, config.ServerPort);
            }
        }
        /// <summary>
        /// 初始化节点状态
        /// </summary>
        /// <param name="state"></param>
        private void _InitState (RpcServiceState state)
        {
            UsableState uState = (UsableState)Interlocked.CompareExchange(ref this._State, 0, 0);
            if (uState == UsableState.停用 && state == RpcServiceState.正常)
            {
                if (this._Reduce.IsEnable)//初始化降级状态
                {
                    this._RefreshTime = HeartbeatTimeHelper.HeartbeatTime + this._Reduce.RefreshTime;
                    _ = Interlocked.Exchange(ref this._SocketErrorNum, 0);
                }
                //初始化限流
                this._Limit.Reset();
                //初始化熔断
                _ = Interlocked.Exchange(ref this._ErrorNum, 0);
                _ = Interlocked.Exchange(ref this._State, 0);
                this._IsUsable = true;
                this._ServiceState = state;
                this._StateChange(uState, UsableState.正常);
            }
            else if (state != RpcServiceState.正常)
            {
                this._ServiceState = state;
                _ = Interlocked.Exchange(ref this._State, 4);
                this._IsUsable = false;
                this._StateChange(uState, UsableState.停用);
            }
        }

        private void _StateChange (UsableState oldState, UsableState state)
        {
            RpcService.Service.RemoteStateChange(this, oldState, state);
        }
        /// <summary>
        /// 每秒刷新节点状态
        /// </summary>
        /// <param name="now"></param>
        public void RefreshState (int now)
        {
            if (this._ServiceState != RpcServiceState.正常)
            {
                return;
            }
            //刷新限流
            this._Limit.Refresh(now);
            if (this._Limit.IsInvalid)//临时限流失效
            {
                this._Limit = new NoEnableLimit();
            }
            if (this._Limit.IsUsable && Interlocked.CompareExchange(ref this._State, 0, 3) == 3)//限流状态恢复
            {
                this._IsUsable = true;
            }
            if (this._Reduce.IsEnable)
            {
                UsableState state = (UsableState)Interlocked.CompareExchange(ref this._State, 0, 0);
                if (state == UsableState.降级 && this._ReduceTime <= now)
                {
                    if (Interlocked.CompareExchange(ref this._State, 0, 2) == 2)
                    {
                        this._IsUsable = true;
                        _ = Interlocked.Exchange(ref this._ErrorNum, 0);
                    }
                }
                if (this._RefreshTime <= now)
                {
                    this._RefreshTime = now + this._Reduce.RefreshTime;
                }
            }
        }
        /// <summary>
        /// 下次检查链接状态时间
        /// </summary>
        private int _CheckTime = 0;
        /// <summary>
        /// 检查节点状态
        /// </summary>
        /// <returns></returns>
        public bool CheckIsUsable (int overTime)
        {
            UsableState state = (UsableState)Interlocked.CompareExchange(ref this._State, 0, 0);
            if (state == UsableState.停用)
            {
                return false;
            }
            else if (state == UsableState.降级)
            {
                return true;
            }
            int now = HeartbeatTimeHelper.HeartbeatTime;
            if (state == UsableState.熔断 && this._CheckTime > now)
            {
                return this.OfflineTime >= overTime;
            }
            else if (this._Client.Ping(out TimeSpan time, out string error))
            {
                this._Refresh(time);
                return true;
            }
            else
            {
                int num = this._SendFail(now, error);
                if (num == 0)
                {
                    return true;
                }
                else if (num <= 3)
                {
                    this._CheckTime = now + 5;
                }
                else
                {
                    this._CheckTime = now + 10;
                }
                return true;
            }
        }
        /// <summary>
        /// 刷新限流配置
        /// </summary>
        public void RefreshLimit ()
        {
            if (!RpcServiceApi.GetServerLimit(this.ServerId, out ServerClientLimit limit, out string error))
            {
                throw new ErrorException(error);
            }
            this._InitLimit(limit);
        }
        /// <summary>
        /// 刷新降级配置
        /// </summary>
        public void RefreshReduce ()
        {
            if (!RpcServiceApi.GetReduceInRank(this.ServerId, this._RpcMerId, out ReduceInRank reduce, out string error))
            {
                throw new ErrorException(error);
            }
            this._Reduce = reduce;
            if (reduce.IsEnable)
            {
                this._RefreshTime = HeartbeatTimeHelper.HeartbeatTime + reduce.RefreshTime;
            }
            _ = Interlocked.Exchange(ref this._SocketErrorNum, 0);
        }
        /// <summary>
        /// 发送文件
        /// </summary>
        /// <param name="config"></param>
        /// <param name="msg"></param>
        /// <param name="file"></param>
        /// <param name="func"></param>
        /// <param name="progress"></param>
        /// <param name="upTask"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool SendFile (IRemoteConfig config, TcpRemoteMsg msg, FileInfo file, UpFileAsync func, UpProgressAction progress, out IUpTask upTask, out string error)
        {
            if (this._Limit.IsLimit())
            {
                this._EnableLimit();
                upTask = null;
                error = "rpc.exceed.limt";
                return false;
            }
            else if (this._Client.SendFile(config.SysDictate, file, msg, func, progress, out upTask, out error))
            {
                return true;
            }
            else if (error == "socket.con.error" || error == "pipe.con.error")
            {
                _ = this._Offline(HeartbeatTimeHelper.HeartbeatTime);
            }
            else
            {
                this._ReduceInRank();
            }
            return false;
        }
        /// <summary>
        /// 获取节点状态
        /// </summary>
        /// <returns></returns>
        public RemoteState GetRemoteState ()
        {
            UsableState state = (UsableState)Interlocked.CompareExchange(ref this._State, 0, 0);
            RemoteState t = new RemoteState
            {
                RemoteId = this.ServerId,
                UsableState = state,
                ConNum = this._Client == null ? 0 : this._Client.GetClientConNum(),
                SendNum = Interlocked.CompareExchange(ref this._SendNum, 0, 0),
                ErrorNum = Interlocked.CompareExchange(ref this._FailSendNum, 0, 0)
            };
            if (t.SendNum != 0)
            {
                long time = Interlocked.Read(ref this._ExecTime);
                t.AvgTime = (int)( time / t.SendNum );
            }
            return t;
        }
        public int GetAvgTime ()
        {
            int sendNum = Interlocked.CompareExchange(ref this._SendNum, 0, 0);
            if (sendNum == 0)
            {
                return 0;
            }
            long time = Interlocked.Read(ref this._ExecTime);
            return (int)( time / sendNum );
        }
        public bool SendData (IRemoteConfig config, TcpRemoteMsg msg, out TcpRemoteReply reply, out string error)
        {
            if (config.IsReply)
            {
                return this._SendData(config.SysDictate, msg, config.TimeOut, out reply, out error);
            }
            else
            {
                reply = null;
                return this._SendData(config.SysDictate, msg, config.TimeOut, out error);
            }
        }

        public bool SendData (string dicate, IRemoteConfig config, TcpRemoteMsg msg, out TcpRemoteReply reply, out string error)
        {
            if (config.IsReply)
            {
                return this._SendData(dicate, msg, config.TimeOut, out reply, out error);
            }
            else
            {
                reply = null;
                return this._SendData(dicate, msg, config.TimeOut, out error);
            }
        }

        #region 私有方法
        private bool _SendData (string dicate, TcpRemoteMsg msg, int? timeout, out string error)
        {
            if (this._Limit.IsLimit())
            {
                this._EnableLimit();
                error = "rpc.exceed.limt";
                return false;
            }
            else if (this._Client.Send(dicate, msg, timeout, out error))
            {
                return true;
            }
            else if (error == "socket.con.error" || error == "pipe.con.error")
            {
                _ = this._Offline(HeartbeatTimeHelper.HeartbeatTime);
            }
            else
            {
                this._ReduceInRank();
            }
            return false;
        }

        private void _Refresh (TimeSpan time)
        {
            if (Interlocked.CompareExchange(ref this._State, 0, 1) == 1)
            {
                this._Recovery();
                this._IsUsable = this._ServiceState == RpcServiceState.正常;
                _ = Interlocked.Exchange(ref this._ErrorNum, 0);
                _ = Interlocked.Exchange(ref this._SendNum, 1);
                _ = Interlocked.Exchange(ref this._ExecTime, time.Ticks);
                this._StateChange(UsableState.熔断, UsableState.正常);
            }
            else
            {
                _ = Interlocked.Exchange(ref this._ErrorNum, 0);
                _ = Interlocked.Increment(ref this._SendNum);
                _ = Interlocked.Add(ref this._ExecTime, time.Ticks);
            }
        }
        /// <summary>
        /// 熔断后恢复
        /// </summary>
        private void _Recovery ()
        {
            //无其它限流规则或恢复限流未配置
            if (this._LimitType != ServerLimitType.不启用 || this._RecoveryLimit == 0 || this._RecoveryTime == 0)
            {
                return;
            }
            //熔断后恢复时实施临时限流
            this._Limit = new RecoveryLimit(this._RecoveryLimit, this._RecoveryTime);
        }
        private void _ReduceInRank ()
        {
            if (!this._Reduce.IsEnable)
            {
                return;
            }
            ReduceInRank config = this._Reduce;
            int num = Interlocked.Increment(ref this._SocketErrorNum);
            if (num >= config.LimitNum)
            {
                _ = Interlocked.Exchange(ref this._SocketErrorNum, 0);
                this._ReduceTime = HeartbeatTimeHelper.HeartbeatTime + Tools.GetRandom(config.BeginDuration, config.EndDuration);
                if (Interlocked.CompareExchange(ref this._State, 2, 0) == 0)
                {
                    this._IsUsable = false;
                    this._StateChange(UsableState.正常, UsableState.降级);
                }
            }
        }
        private void _EnableLimit ()
        {
            if (Interlocked.CompareExchange(ref this._State, 3, 0) == 0)
            {
                this._IsUsable = false;
                this._StateChange(UsableState.正常, UsableState.限流);
            }
        }
        private bool _SendData (string dictate, TcpRemoteMsg msg, int? timeout, out TcpRemoteReply reply, out string error)
        {
            if (this._Limit.IsLimit())
            {
                this._EnableLimit();
                reply = null;
                error = "rpc.exceed.limt";
                return false;
            }
            else if (this._Client.Send(dictate, msg, timeout, out reply, out error))
            {
                return true;
            }
            else if (error == "socket.con.error" || error == "pipe.con.error")
            {
                _ = this._Offline(HeartbeatTimeHelper.HeartbeatTime);
            }
            else
            {
                this._ReduceInRank();
            }
            return false;
        }
        private void _InitLimit (ServerClientLimit limit)
        {
            this._LimitType = limit.LimitType;
            if (limit.LimitType == ServerLimitType.不启用)
            {
                this._Limit = new Limit.NoEnableLimit();
            }
            else if (limit.LimitType == ServerLimitType.固定时间窗)
            {
                this._Limit = new Limit.FixedTimeLimit(limit.LimitNum, limit.LimitTime);
            }
            else if (limit.LimitType == ServerLimitType.流动时间窗)
            {
                this._Limit = new Limit.SlideTimeLimit(limit.LimitNum, limit.LimitTime);
            }
            else
            {
                this._Limit = new Limit.TokenLimit(limit.TokenNum, limit.TokenInNum);
            }
            if (Interlocked.CompareExchange(ref this._State, 0, 3) == 3)
            {
                this._StateChange(UsableState.限流, UsableState.正常);
            }
        }
        private int _SendFail (int now, string error)
        {
            _ = Interlocked.Increment(ref this._FailSendNum);
            if (error == "socket.con.error" || error == "pipe.con.error")
            {
                return this._Offline(now);
            }
            return 0;
        }
        private int _Offline (int now)
        {
            int num = Interlocked.Increment(ref this._ErrorNum);
            int state = Interlocked.CompareExchange(ref this._State, 0, 0);
            if (state == 0 && num == this._Reduce.FusingErrorNum)
            {
                this.OfflineTime = now;
                if (Interlocked.Exchange(ref this._State, 1) == 0)
                {
                    this._IsUsable = false;
                    this._StateChange(UsableState.正常, UsableState.熔断);
                }
            }
            return num;
        }
        #endregion
    }
}