using System;
using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Config;
using WeDonekRpc.Client.Log;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using WeDonekRpc.Model.Model;
using WeDonekRpc.Model.Server;
using WeDonekRpc.TcpClient.Config;

namespace WeDonekRpc.Client.Server
{
    internal class RpcServer
    {
        private const int _DefPort = 983;

        public RpcServer (string ip)
        {
            if (!ip.Contains(':'))
            {
                this.ServerIp = ip;
                this.ServerPort = RpcServer._DefPort;
            }
            else
            {
                string[] t = ip.Split(':');
                this.ServerIp = t[0];
                this.ServerPort = int.Parse(t[1]);
            }
            this.ServerId = Guid.NewGuid().ToString("N");
            string clientId = string.Join("_", WebConfig.Environment.Mac, WebConfig.BasicConfig.RpcServerIndex, WebConfig.BasicConfig.RpcSystemType).GetMd5().ToLower();
            SocketConfig.AddConArg(this.ServerIp, this.ServerPort, null, WebConfig.BasicConfig.AppId, clientId);
            this._RemoteClient = new TcpClient.TcpClient(this.ServerIp, this.ServerPort);
            this.IsUsable = true;
        }
        public RpcServer (string ip, int port)
        {
            this.ServerId = Guid.NewGuid().ToString("N");
            this.ServerIp = ip;
            this.ServerPort = port == 0 ? _DefPort : port;
            string clientId = string.Join("_", WebConfig.Environment.Mac, WebConfig.BasicConfig.RpcServerIndex, WebConfig.BasicConfig.RpcSystemType).GetMd5().ToLower();
            SocketConfig.AddConArg(this.ServerIp, this.ServerPort, null, WebConfig.BasicConfig.AppId, clientId);
            this._RemoteClient = new TcpClient.TcpClient(this.ServerIp, this.ServerPort);
            this.IsUsable = true;
        }
        public string ServerId
        {
            get;
        }
        public bool IsUsable
        {
            get;
            private set;
        }
        public string ServerIp
        {
            get;
            set;
        }

        public int ServerPort
        {
            get;
            set;
        }
        private readonly TcpClient.TcpClient _RemoteClient = null;
        public bool CheckIsUsable ()
        {
            if (this.IsUsable)
            {
                this.IsUsable = this._RemoteClient.CheckIsUsable(out _);
                return this.IsUsable;
            }
            return false;
        }
        public void CheckIsRestore ()
        {
            if (!this.IsUsable)
            {
                this.IsUsable = this._RemoteClient.CheckIsUsable(out _);
            }
        }
        public bool GetAccessToken (out AccessToken token, out string error)
        {
            ApplyAccessToken obj = new ApplyAccessToken
            {
                AppId = WebConfig.BasicConfig.AppId,
                Secret = WebConfig.BasicConfig.AppSecret
            };
            if (this._Send("GetAccessToken", obj, out AccessTokenRes res, out error))
            {
                token = new AccessToken(ref res);
                return true;
            }
            else
            {
                token = null;
                return false;
            }
        }
        public bool GetControlServer (out RpcControlServer[] servers, out string error)
        {
            return this._Send("GetControlServer", out servers, out error);
        }
        public bool GetServerLimit (long serverId, out ServerClientLimit limit, out string error)
        {
            GetClientLimit obj = new GetClientLimit
            {
                ServerId = serverId,
                RpcMerId = RpcStateCollect.RpcMerId
            };
            return this._Send("GetClientLimit", obj, out limit, out error);
        }
        public bool GetReduceInRank (long serverId, long rpcMerId, out ReduceInRank reduce, out string error)
        {
            GetReduceInRank obj = new GetReduceInRank
            {
                ServerId = serverId,
                RpcMerId = rpcMerId
            };
            return this._Send("GetReduceInRank", obj, out reduce, out error);
        }
        internal bool SendHeartbeat (ServiceHeartbeat obj, out int verNum, out string error)
        {
            return this._Send("ServiceHeartbeat", obj, out verNum, out error);
        }
        public bool GetServerLimit (RpcToken token, out LimitConfigRes config, out string error)
        {
            GetServerLimit obj = new GetServerLimit
            {
                AccessToken = token.Access_Token
            };
            if (this._Send("GetServerLimit", obj, out config, out error))
            {
                return true;
            }
            else if (error.StartsWith("rpc.token."))
            {
                if (token.ResetToken())
                {
                    return this.GetServerLimit(token, out config, out error);
                }
            }
            return false;
        }
        public bool ServerLogin (RpcToken token, RemoteServerLogin login, out RpcConfig config, out RpcServerConfig serverConfig, out string error)
        {
            RpcServerLogin obj = new RpcServerLogin
            {
                AccessToken = token.Access_Token,
                ServerLogin = login,
                ApiVer = WebConfig.ApiVerNum
            };
            string key = login.ContainerType == ContainerType.无 ? "ServerLogin" : "ContrainerLogin";
            if (this._Send(key, obj, out RpcServerLoginRes model, out error))
            {
                config = model.OAuthConfig;
                serverConfig = model.ServerConfig;
                return true;
            }
            else if (error.StartsWith("rpc.token."))
            {
                if (token.ResetToken())
                {
                    return this.ServerLogin(token, login, out config, out serverConfig, out error);
                }
            }
            config = null;
            serverConfig = null;
            return false;
        }
        private bool _Send<T> (string type, T data, out string error) where T : class
        {
            if (!this._SendMsg(type, data, out error))
            {
                RpcLogSystem.AddMsgErrorLog(type, data, error, this);
                return false;
            }
            RpcLogSystem.AddMsgLog(type, data, this);
            return true;
        }
        private bool _SendMsg<T> (string type, T data, out string error) where T : class
        {
            if (!this._RemoteClient.Send(type, data, out BasicRes res, out error))
            {
                return false;
            }
            else if (res.IsError)
            {
                error = res.ErrorMsg;
                return false;
            }
            else
            {
                return true;
            }
        }
        private bool _Send<Result> (string type, out Result result, out string error)
        {
            if (!this._SendMsg(type, out result, out error))
            {
                RpcLogSystem.AddMsgErrorLog(type, error, this);
                return false;
            }
            RpcLogSystem.AddMsgLog(type, result, this);
            return true;
        }
        private bool _Send<T, Result> (string type, T data, out Result result, out string error) where T : class
        {
            if (!this._SendMsg(type, data, out result, out error))
            {
                RpcLogSystem.AddMsgErrorLog(type, data, error, this);
                return false;
            }
            RpcLogSystem.AddMsgLog(type, data, ref result, this);
            return true;
        }
        private bool _SendMsg<T, Result> (string type, T data, out Result result, out string error) where T : class
        {
            if (!this._RemoteClient.Send(type, data, out BasicRes<Result> res, out error))
            {
                result = default;
                return false;
            }
            else if (res == null)
            {
                result = default;
                error = "rpc.data.return.null";
                return false;
            }
            else if (res.IsError)
            {
                result = default;
                error = res.ErrorMsg;
                return false;
            }
            else
            {
                result = res.Result;
                return true;
            }
        }
        private bool _SendMsg<Result> (string type, out Result result, out string error)
        {
            if (!this._RemoteClient.Send(type, out BasicRes<Result> res, out error))
            {
                result = default;
                return false;
            }
            else if (res == null)
            {
                result = default;
                error = "rpc.data.return.null";
                return false;
            }
            else if (res.IsError)
            {
                result = default;
                error = res.ErrorMsg;
                return false;
            }
            else
            {
                result = res.Result;
                return true;
            }
        }

        public bool GetServerConfig (RpcToken token, long serverId, out ServerConfigInfo config, out string error)
        {
            GetRemoteServer obj = new GetRemoteServer
            {
                CurServerId = RpcStateCollect.ServerId,
                ServerId = serverId
            };
            if (this._Send("GetRemoteServer", obj, out config, out error))
            {
                return true;
            }
            else if (error.StartsWith("rpc.token."))
            {
                if (token.ResetToken())
                {
                    return this.GetServerConfig(token, serverId, out config, out error);
                }
            }
            config = null;
            return false;
        }


        public bool GetRemoteServer (RemoteBody body, int ver, string tVer, out GetServerListRes res, out string error)
        {
            GetServerList obj = new GetServerList
            {
                TransmitVer = tVer,
                SystemType = body.systemType,
                LimitRegionId = body.regionId,
                RpcMerId = body.rpcMerId,
                ConfigVer = ver,
                Source = new Source
                {
                    RegionId = RpcStateCollect.ServerConfig.RegionId,
                    Ver = RpcStateCollect.ServerConfig.VerNum,
                    RpcMerId = RpcStateCollect.RpcMerId,
                    SystemTypeId = RpcStateCollect.ServerConfig.SystemType
                }
            };
            return this._Send("GetServerList", obj, out res, out error);
        }

        internal bool RefreshRpc (RpcToken token, string key, RefreshEventParam param, out string error)
        {
            RefreshRpc obj = new RefreshRpc
            {
                EventKey = key,
                Param = param,
                TokenId = token.Access_Token
            };
            if (this._Send("RefreshRpc", obj, out error))
            {
                return true;
            }
            else if (error.StartsWith("rpc.token."))
            {
                if (token.ResetToken())
                {
                    return this.RefreshRpc(token, key, param, out error);
                }
            }
            return false;
        }
    }
}
