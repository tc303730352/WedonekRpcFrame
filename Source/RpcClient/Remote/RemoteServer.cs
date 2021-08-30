using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using RpcClient.Collect;
using RpcClient.Interface;
using RpcClient.RpcApi;

using RpcModel;
using RpcModel.Model;
using RpcModel.Server;

using SocketTcpClient;
using SocketTcpClient.UpFile;

using RpcHelper;
namespace RpcClient.Remote
{
        /// <summary>
        /// 远程节点服务
        /// </summary>
        internal class RemoteServer : DataSyncClass, IRemoteHelper
        {

                public RemoteServer(long serverId)
                {
                        this._ServerId = serverId;
                }
                /// <summary>
                /// 服务节点Id
                /// </summary>
                private readonly long _ServerId = 0;
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
                /// TCP 客户端对象
                /// </summary>
                private TcpClient _Client = null;

                /// <summary>
                /// 执行时长
                /// </summary>
                private int _ExecTime = 0;

                /// <summary>
                /// 发送量
                /// </summary>
                private int _SendNum = 0;
                /// <summary>
                /// 累计的传输级别的错误量
                /// </summary>
                private int _SocketErrorNum = 0;
                /// <summary>
                /// 节点类型Id
                /// </summary>
                private long _SystemType = 0;
                /// <summary>
                /// 降级熔断配置
                /// </summary>
                private ReduceInRank _Reduce = null;
                /// <summary>
                /// 计算连续发生链接错误量（用于熔断）
                /// </summary>
                private int _ErrorNum = 0;

                /// <summary>
                /// 节点限制
                /// </summary>
                private IServerLimit _Limit = null;

                /// <summary>
                /// 服务器ID
                /// </summary>
                public long ServerId => this._ServerId;
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
                public long SystemType => this._SystemType;
                /// <summary>
                /// 服务名称
                /// </summary>
                public string ServerName { get; private set; }

                /// <summary>
                /// 节点状态
                /// </summary>
                private RpcServiceState _ServiceState = RpcServiceState.正常;
                protected override bool SyncData()
                {
                        if (!RpcTokenCollect.GetAccessToken(out RpcToken token, out string error))
                        {
                                this.Error = error;
                                return false;
                        }
                        else if (!RpcServiceApi.GetRemoteServerData(token, this._ServerId, out ServerConfigInfo config, out error))
                        {
                                this.Error = error;
                                return false;
                        }
                        else if (config == null)
                        {
                                this.Error = "rpc.server.config.get.error";
                                return false;
                        }
                        else
                        {
                                this.RegionId = config.RegionId;
                                this._SystemType = config.SystemType;
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
                                                this._RefreshTime = (HeartbeatTimeHelper.HeartbeatTime + this._Reduce.RefreshTime);
                                        }
                                }
                                //添加建立链接的必要参数
                                SocketTcpClient.Config.SocketConfig.AddConArg(config.ServerIp, config.ServerPort, config.PublicKey);
                                if (this._Client == null)
                                {
                                        this._Client = new TcpClient(config.ServerIp, config.ServerPort);
                                        this.ServerIp = config.ServerIp;
                                        this.Port = config.ServerPort;
                                }
                                this._InitState(config.ServiceState);
                                return true;
                        }
                }
                /// <summary>
                /// 初始化节点状态
                /// </summary>
                /// <param name="state"></param>
                private void _InitState(RpcServiceState state)
                {
                        int uState = Interlocked.CompareExchange(ref this._State, 0, 0);
                        if (uState == 4 && state == RpcServiceState.正常)
                        {
                                if (this._Reduce.IsEnable)//初始化降级状态
                                {
                                        this._RefreshTime = (HeartbeatTimeHelper.HeartbeatTime + this._Reduce.RefreshTime);
                                        Interlocked.Exchange(ref this._SocketErrorNum, 0);
                                }
                                //初始化限流
                                this._Limit.Reset();
                                //初始化熔断
                                Interlocked.Exchange(ref this._ErrorNum, 0);
                                Interlocked.Exchange(ref this._State, 0);
                                this._IsUsable = true;
                                this._ServiceState = state;
                                RpcLogSystem.AddRemoteLog(this, uState);
                        }
                        else if (state != RpcServiceState.正常)
                        {
                                this._ServiceState = state;
                                Interlocked.Exchange(ref this._State, 4);
                                this._IsUsable = false;
                                RpcLogSystem.AddRemoteLog(this, uState);
                        }
                }
                /// <summary>
                /// 每秒刷新节点状态
                /// </summary>
                /// <param name="now"></param>
                public void RefreshState(int now)
                {
                        if (this._ServiceState != RpcServiceState.正常)
                        {
                                return;
                        }
                        //刷新限流
                        this._Limit.Refresh(now);
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
                                                Interlocked.Exchange(ref this._ErrorNum, 0);
                                        }
                                }
                                if (this._RefreshTime <= now)
                                {
                                        this._RefreshTime = (now + this._Reduce.RefreshTime);
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
                public bool CheckIsUsable()
                {
                        UsableState state = (UsableState)Interlocked.CompareExchange(ref this._State, 0, 0);
                        if (state == UsableState.降级 || state == UsableState.停用)
                        {
                                return true;
                        }
                        int now = HeartbeatTimeHelper.HeartbeatTime;
                        if (state == UsableState.熔断 && this._CheckTime > now)
                        {
                                return false;
                        }
                        else if (this._Client.Ping(out TimeSpan time))
                        {
                                this._Refresh(time);
                                return true;
                        }
                        int num = this._Offline(now);
                        if (num <= 3)
                        {
                                this._CheckTime = now + 5;
                        }
                        else
                        {
                                this._CheckTime = now + 10;
                        }
                        return false;
                }
                /// <summary>
                /// 刷新限流配置
                /// </summary>
                public void RefreshLimit()
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
                public void RefreshReduce()
                {
                        if (!RpcServiceApi.GetReduceInRank(this.ServerId, out ReduceInRank reduce, out string error))
                        {
                                throw new ErrorException(error);
                        }
                        this._Reduce = reduce;
                        if (reduce.IsEnable)
                        {
                                this._RefreshTime = (HeartbeatTimeHelper.HeartbeatTime + reduce.RefreshTime);
                        }
                        Interlocked.Exchange(ref this._SocketErrorNum, 0);
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
                public bool SendFile(IRemoteConfig config, TcpRemoteMsg msg, FileInfo file, UpFileAsync func, UpProgress progress, out UpTask upTask, out string error)
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
                        else if (error == "socket.con.error")
                        {
                                this._Offline(HeartbeatTimeHelper.HeartbeatTime);
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
                public RemoteState GetRemoteState()
                {
                        UsableState state = (UsableState)Interlocked.CompareExchange(ref this._State, 0, 0);
                        return new RemoteState
                        {
                                ServerId = this.ServerId,
                                AvgTime = this.GetAvgTime(),
                                State = state,
                                ConNum = this._Client == null ? 0 : this._Client.GetClientConNum(),
                                SendNum = this._SendNum,
                                ErrorNum = Interlocked.CompareExchange(ref this._SocketErrorNum, 0, 0)
                        };
                }
                /// <summary>
                /// 获取平均响应时间
                /// </summary>
                /// <returns></returns>
                public int GetAvgTime()
                {
                        if (this._SendNum == 0)
                        {
                                return 0;
                        }
                        return this._ExecTime / this._SendNum;
                }

                public bool SendData(IRemoteConfig config, TcpRemoteMsg msg, out TcpRemoteReply reply, out string error)
                {
                        if (config.IsReply)
                        {
                                return this._SendData(config.SysDictate, msg, out reply, out error);
                        }
                        else
                        {
                                reply = null;
                                return this._SendData(config.SysDictate, msg, out error);
                        }
                }

                public bool SendData(string dicate, IRemoteConfig config, TcpRemoteMsg msg, out TcpRemoteReply reply, out string error)
                {
                        if (config.IsReply)
                        {
                                return this._SendData(dicate, msg, out reply, out error);
                        }
                        else
                        {
                                reply = null;
                                return this._SendData(dicate, msg, out error);
                        }
                }

                #region 私有方法
                private bool _SendData(string dicate, TcpRemoteMsg msg, out string error)
                {
                        if (this._Limit.IsLimit())
                        {
                                this._EnableLimit();
                                error = "rpc.exceed.limt";
                                return false;
                        }
                        else if (this._Client.Send(dicate, msg, out error))
                        {
                                return true;
                        }
                        else if (error == "socket.con.error")
                        {
                                this._Offline(HeartbeatTimeHelper.HeartbeatTime);
                        }
                        else
                        {
                                this._ReduceInRank();
                        }
                        return false;
                }

                private void _Refresh(TimeSpan time)
                {
                        if (Interlocked.CompareExchange(ref this._State, 0, 1) == 1)
                        {
                                this._IsUsable = this._ServiceState == RpcServiceState.正常;
                        }
                        Interlocked.Exchange(ref this._ErrorNum, 0);
                        this._ExecTime += (int)time.TotalMilliseconds;
                        this._SendNum += 1;
                }
                private void _ReduceInRank()
                {
                        if (!this._Reduce.IsEnable)
                        {
                                return;
                        }
                        ReduceInRank config = this._Reduce;
                        int num = Interlocked.Increment(ref this._SocketErrorNum);
                        if (num >= config.LimitNum)
                        {
                                Interlocked.Exchange(ref this._SocketErrorNum, 0);
                                this._ReduceTime = (HeartbeatTimeHelper.HeartbeatTime + Tools.GetRandom(config.BeginDuration, config.EndDuration));
                                if (Interlocked.CompareExchange(ref this._State, 2, 0) == 0)
                                {
                                        this._IsUsable = false;
                                        RpcLogSystem.AddRemoteLog(this, 0);
                                }
                        }
                }
                private void _EnableLimit()
                {
                        if (Interlocked.CompareExchange(ref this._State, 3, 0) == 0)
                        {
                                this._IsUsable = false;
                                RpcLogSystem.AddRemoteLog(this, 0);
                        }
                }
                private bool _SendData(string dictate, TcpRemoteMsg msg, out TcpRemoteReply reply, out string error)
                {
                        if (this._Limit.IsLimit())
                        {
                                this._EnableLimit();
                                reply = null;
                                error = "rpc.exceed.limt";
                                return false;
                        }
                        else if (this._Client.Send(dictate, msg, out reply, out error))
                        {
                                return true;
                        }
                        else if (error == "socket.con.error")
                        {
                                this._Offline(HeartbeatTimeHelper.HeartbeatTime);
                        }
                        else
                        {
                                this._ReduceInRank();
                        }
                        return false;
                }
                private void _InitLimit(ServerClientLimit limit)
                {
                        if (!limit.IsEnable)
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
                        else if (limit.LimitType == ServerLimitType.令牌桶)
                        {
                                this._Limit = new Limit.TokenLimit(limit.TokenNum, limit.TokenInNum);
                        }
                        else
                        {
                                this._Limit = new Limit.NoEnableLimit();
                        }
                        if (Interlocked.CompareExchange(ref this._State, 0, 3) == 3)
                        {
                                RpcLogSystem.AddRemoteLog(this, 3);
                        }
                }
                private int _Offline(int now)
                {
                        int num = Interlocked.Increment(ref this._ErrorNum);
                        int state = Interlocked.CompareExchange(ref this._State, 0, 0);
                        if (state == 0 && num == this._Reduce.FusingErrorNum)
                        {
                                this.OfflineTime = now;
                                if (Interlocked.Exchange(ref this._State, 1) == 0)
                                {
                                        this._IsUsable = false;
                                        RpcLogSystem.AddRemoteLog(this, 0);
                                }
                        }
                        return num;
                }
                #endregion
                public override string ToString()
                {
                        return string.Format("state:{0}\r\nip:{1}:{2}\r\nServerId:{3}\r\nName:{4}\r\nOffline:{5}\r\nRegionId:{6}\r\nServerState:{7}", (UsableState)this._State,
                                         this.ServerIp,
                                         this.Port,
                                         this.ServerId,
                                         this.ServerName,
                                         this.OfflineTime,
                                         this.RegionId,
                                         this._ServiceState);
                }
                public string ToString(int state)
                {
                        return string.Format("source:{7}\r\nstate:{0}\r\nip:{1}:{2}\r\nServerId:{3}\r\nName:{4}\r\nOffline:{5}\r\nRegionId:{6}\r\nServerState:{8}", (UsableState)this._State,
                                         this.ServerIp,
                                         this.Port,
                                         this.ServerId,
                                         this.ServerName,
                                         this.OfflineTime,
                                         this.RegionId,
                                         (UsableState)state,
                                         this._ServiceState);
                }

                public Dictionary<string, dynamic> ToDictionary(int state)
                {
                        Dictionary<string, dynamic> dic = this.ToDictionary();
                        dic.Add("OldUsableState", state);
                        return dic;
                }

                public Dictionary<string, dynamic> ToDictionary()
                {
                        return new Dictionary<string, dynamic>
                       {
                               { "UsableState", this._State},
                               { "Ip",this.ServerIp},
                               { "Port",this.Port},
                               { "Id",this.ServerId},
                               { "Name",this.ServerName},
                               { "Offline",this.OfflineTime},
                               { "RegionId",this.RegionId},
                               { "nServerState",this._ServiceState},
                        };
                }
        }
}