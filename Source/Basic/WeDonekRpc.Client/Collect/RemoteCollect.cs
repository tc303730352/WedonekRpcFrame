using System;
using System.Collections.Generic;
using System.IO;
using WeDonekRpc.Client.Config;
using WeDonekRpc.Client.Helper;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.IOSendInterface;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Collect
{
    public class RemoteCollect
    {
        private static readonly IRemoteSend _Remote = null;

        static RemoteCollect ()
        {
            _Remote = new Send.RemoteSend();
        }
        public static BasicResult<Result>[] MultipleSend<T, Result> (T data) where T : class
        {
            Type type = data.GetType();
            if (!RemoteConfigCache.GetRemoteConfig(type, out IRemoteConfig config))
            {
                throw new ErrorException("rpc.class.config.error");
            }
            else
            {
                IRemoteResult[] results = _Remote.MultipleSend(data, config);
                return results.ConvertAll(RpcClientHelper.GetResult<Result>);
            }
        }

        #region 发送数据
        public static bool Send<T> (long serverId, T data, out string error) where T : class
        {
            IRemoteResult result = _Send<T>(serverId, data);
            return RpcClientHelper.GetResult(result, out error);
        }
        public static bool Send<T> (long rpcMerId, int? regionId, T data, out string error) where T : class
        {
            IRemoteResult result = _Send<T>(data, regionId, rpcMerId);
            return RpcClientHelper.GetResult(result, out error);
        }

        public static bool Send (IRemoteConfig config, Dictionary<string, object> dic, out string error)
        {
            return Send(config, new DynamicModel(dic), out error);
        }
        public static bool Send (IRemoteConfig config, DynamicModel obj, out string error)
        {
            config.InitConfig(WebConfig.RpcConfig.MaxRetryNum);
            IRemoteResult result = _Remote.Send(config, obj);
            return RpcClientHelper.GetResult(result, out error);
        }
        public static bool Send<T> (IRemoteConfig config, T data, out string error) where T : class
        {
            config.InitConfig(WebConfig.RpcConfig.MaxRetryNum);
            IRemoteResult result = _Remote.Send(config, data);
            return RpcClientHelper.GetResult(result, out error);
        }
        public static bool Send<Result> (IRemoteConfig config, out Result result, out string error)
        {
            config.InitConfig(WebConfig.RpcConfig.MaxRetryNum);
            IRemoteResult res = _Remote.Send(config, null);
            if (!RpcClientHelper.GetResult(res, out BasicRes<Result> data, out error))
            {
                result = default;
                return false;
            }
            else
            {
                result = data.Result;
                return true;
            }
        }
        public static bool Send<T> (string sysType, T data, out string error)
        {
            IRemoteResult result = _Send<T>(data, sysType);
            return RpcClientHelper.GetResult(result, out error);
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Result"></typeparam>
        /// <param name="model"></param>
        /// <param name="result"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool Send<T, Result> (T model, out Result result, out string error)
        {
            if (!_Send(model, out BasicRes<Result> res, out error))
            {
                result = default;
                return false;
            }
            else
            {
                result = res.Result;
                return true;
            }
        }

        public static bool Send<T, Result> (long rpcMerId, int regionId, T model, out Result result, out string error)
        {
            if (!_Send(model, regionId, rpcMerId, out BasicRes<Result> res, out error))
            {
                result = default;
                return false;
            }
            else
            {
                result = res.Result;
                return true;
            }
        }
        public static bool Send<T, Result> (T model, out Result result, out long serverId, out string error)
        {
            if (!_Send(model, out BasicRes<Result> res, out serverId, out error))
            {
                result = default;
                return false;
            }
            else
            {
                result = res.Result;
                return true;
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Result"></typeparam>
        /// <param name="model"></param>
        /// <param name="sysType"></param>
        /// <param name="result"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool Send<T, Result> (T model, string sysType, out Result result, out string error)
        {
            IRemoteResult sendRes = _Send(model, sysType);
            if (!RpcClientHelper.GetResult(sendRes, out BasicRes<Result> res, out error))
            {
                result = default;
                return false;
            }
            else
            {
                result = res.Result;
                return true;
            }
        }
        public static bool Send<T, Result> (long serverId, T model, out Result result, out string error)
        {
            if (!_Send(serverId, model, out BasicRes<Result> res, out error))
            {
                result = default;
                return false;
            }
            else
            {
                result = res.Result;
                return true;
            }
        }
        /// <summary>
        /// 发送数据并返回结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool Send<T> (T model, out string error)
        {
            IRemoteResult res = _Send<T>(model);
            return RpcClientHelper.GetResult(res, out error);
        }

        public static bool Send<T> (T model, int regionId, long rpcMerId, out string error)
        {
            IRemoteResult res = _Send<T>(model, regionId, rpcMerId);
            return RpcClientHelper.GetResult(res, out error);
        }

        public static bool SendFile<T> (T model, FileInfo file, UpFileAsync func, UpProgressAction progress, out IUpTask upTask, out string error)
        {
            Type type = model.GetType();
            if (!RemoteConfigCache.GetRemoteConfig(type, out IRemoteConfig config))
            {
                upTask = null;
                error = "rpc.class.config.error";
                return false;
            }
            else if (!RpcClientHelper.ValidateData(config, model, out error))
            {
                upTask = null;
                return false;
            }
            else if (!RemoteServerNode.FindServer<T>(config.SystemType, config, model, out IRemote remote))
            {
                upTask = null;
                error = "rpc.no.server";
                return false;
            }
            else
            {
                TcpRemoteMsg msg = RpcClientHelper.GetTcpRemoteMsg<T>(model, type, config);
                return remote.SendFile(config, msg, file, func, progress, out upTask, out error);
            }
        }
        public static bool SendFile<T> (long serverId, T model, FileInfo file, UpFileAsync func, UpProgressAction progress, out IUpTask upTask, out string error)
        {
            Type type = model.GetType();
            if (!RemoteConfigCache.GetRemoteConfig(type, out IRemoteConfig config))
            {
                upTask = null;
                error = "rpc.class.config.error";
                return false;
            }
            else if (!RpcClientHelper.ValidateData(config, model, out error))
            {
                upTask = null;
                return false;
            }
            else if (!RemoteServerCollect.GetUsableServer(serverId, config.IsProhibitTrace, out IRemote remote))
            {
                upTask = null;
                error = "rpc.remote.not.find";
                return false;
            }
            else
            {
                TcpRemoteMsg msg = RpcClientHelper.GetTcpRemoteMsg<T>(model, type, config);
                return remote.SendFile(config, msg, file, func, progress, out upTask, out error);
            }
        }

        public static bool Send (long serverId, BroadcastDatum msg, MsgSource source, out string error)
        {
            return _Send(new IRemoteConfig(msg), msg, serverId, source, out error);
        }

        public static IRemoteResult SendData (string sysType, BroadcastDatum msg)
        {
            return _Send(sysType, msg, RpcStateCollect.LocalSource);
        }
        public static bool Send (string sysType, BroadcastDatum msg, MsgSource source, out string error)
        {
            IRemoteResult result = _Send(sysType, msg, source);
            return RpcClientHelper.GetResult(result, out error);
        }

        public static bool SendStream<T, Result> (T model, byte[] stream, out Result result, out string error)
        {
            if (!_SendStream(model, stream, out BasicRes<Result> res, out error))
            {
                result = default;
                return false;
            }
            else
            {
                result = res.Result;
                return true;
            }
        }
        public static bool SendStream<T> (T model, byte[] stream, out string error)
        {
            return _SendStream(model, stream, out error);
        }
        #endregion

        #region 发送的方法
        private static bool _Send<T, Result> (long serverId, T model, out Result result, out string error) where Result : IBasicRes
        {
            IRemoteResult res = _Send<T>(serverId, model);
            return RpcClientHelper.GetResult(res, out result, out error);
        }


        private static IRemoteResult _Send<T> (T model, int? regionId, long rpcMerId)
        {
            Type type = model.GetType();
            if (!RemoteConfigCache.GetRemoteConfig(type, out IRemoteConfig config))
            {
                return new IRemoteResult("rpc.class.config.error");
            }
            else if (!RpcClientHelper.ValidateData(config, model, out string error))
            {
                return new IRemoteResult(error);
            }
            else
            {
                config = config.Clone();
                config.RegionId = regionId;
                config.RpcMerId = rpcMerId;
                return _Remote.Send(config, model);
            }
        }
        private static IRemoteResult _Send<T> (T model)
        {
            Type type = model.GetType();
            if (!RemoteConfigCache.GetRemoteConfig(type, out IRemoteConfig config))
            {
                return new IRemoteResult("rpc.class.config.error");
            }
            else
            {
                return !RpcClientHelper.ValidateData(config, model, out string error) ? new IRemoteResult(error) : _Remote.Send(config, model);
            }
        }

        private static IRemoteResult _Send<T> (long serverId, T model)
        {
            Type type = model.GetType();
            if (!RemoteConfigCache.GetRemoteConfig(type, out IRemoteConfig config))
            {
                return new IRemoteResult("rpc.class.config.error");
            }
            else
            {
                return !RpcClientHelper.ValidateData(config, model, out string error)
                    ? new IRemoteResult(error)
                    : _Remote.Send(config, serverId, model);
            }
        }

        private static IRemoteResult _Send<T> (T model, string sysType)
        {
            Type type = model.GetType();
            if (!RemoteConfigCache.GetRemoteConfig(type, out IRemoteConfig config))
            {
                return new IRemoteResult("rpc.class.config.error");
            }
            else
            {
                return !RpcClientHelper.ValidateData(config, model, out string error)
                    ? new IRemoteResult(error)
                    : _Remote.Send(config, sysType, model);
            }
        }
        private static bool _Send<T, Result> (T model, int regionId, long rpcMerId, out Result result, out string error) where Result : IBasicRes
        {
            IRemoteResult res = _Send<T>(model, regionId, rpcMerId);
            return RpcClientHelper.GetResult(res, out result, out error);
        }
        private static bool _Send<T, Result> (T model, out Result result, out string error) where Result : IBasicRes
        {
            IRemoteResult res = _Send<T>(model);
            return RpcClientHelper.GetResult(res, out result, out error);
        }
        private static bool _Send<T, Result> (T model, out Result result, out long serverId, out string error) where Result : IBasicRes
        {
            IRemoteResult res = _Send(model);
            serverId = res.RemoteServerId;
            return RpcClientHelper.GetResult(res, out result, out error);
        }
        private static IRemoteResult _Send (string sysType, BroadcastDatum msg, MsgSource source)
        {
            IRemoteConfig config = new IRemoteConfig(msg, sysType, source.ServerId);
            config.InitConfig(WebConfig.RpcConfig.MaxRetryNum);
            return _Remote.Send(config, msg, source);
        }
        private static bool _Send (IRemoteConfig config, BroadcastDatum broadcast, long serverId, MsgSource source, out string error)
        {
            config.InitConfig(WebConfig.RpcConfig.MaxRetryNum);
            IRemoteResult result = _Remote.Send(config, broadcast, serverId, source);
            if (result.IsError)
            {
                error = result.ErrorMsg;
                return false;
            }
            error = null;
            return true;
        }
        /// <summary>
        /// 发送数据流（小文件）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Result"></typeparam>
        /// <param name="model"></param>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        private static bool _SendStream<T, Result> (T model, byte[] stream, out Result result, out string error) where Result : IBasicRes
        {
            IRemoteResult res = _SendStream<T>(model, stream);
            return RpcClientHelper.GetResult(res, out result, out error);
        }
        private static bool _SendStream<T> (T model, byte[] stream, out string error)
        {
            IRemoteResult res = _SendStream<T>(model, stream);
            return RpcClientHelper.GetResult(res, out error);
        }
        private static IRemoteResult _SendStream<T> (T model, byte[] stream)
        {
            Type type = model.GetType();
            if (!RemoteConfigCache.GetRemoteConfig(type, out IRemoteConfig config))
            {
                return new IRemoteResult("public.class.config.error");
            }
            else
            {
                return !RpcClientHelper.ValidateData(config, model, out string error)
                    ? new IRemoteResult(error)
                    : _Remote.SendStream(config, model, stream);
            }
        }
        #endregion

        #region 消息广播


        public static void BroadcastMsg (string dictate, params long[] serverId)
        {
            if (string.IsNullOrEmpty(dictate))
            {
                throw new ErrorException("rpc.dictate.null");
            }
            IRemoteBroadcast config = new(dictate, false, serverId)
            {
                BroadcastType = BroadcastType.默认,
                IsCrossGroup = true,
                IsOnly = false,
                RpcMerId = RpcClient.CurrentSource.RpcMerId
            };
            BroadcastCollect.GetBroadcast(config).BroadcastMsg(config, string.Empty);
        }
        public static void BroadcastMsg (IRemoteBroadcast config, Dictionary<string, object> body)
        {
            BroadcastMsg(config, new DynamicModel(body));
        }
        public static void BroadcastMsg (IRemoteBroadcast config, DynamicModel body)
        {
            _InitBroadcast(config);
            BroadcastCollect.GetBroadcast(config).BroadcastMsg(config, body);
        }
        public static void BroadcastMsg<T> (IRemoteBroadcast config, T msg)
        {
            _InitBroadcast(config);
            BroadcastCollect.GetBroadcast(config).BroadcastMsg(config, msg);
        }
        public static bool BroadcastMsg<T> (IRemoteBroadcast config, T msg, out string error)
        {
            _InitBroadcast(config);
            return BroadcastCollect.GetBroadcast(config).BroadcastMsg(config, msg, out error);
        }
        public static void BroadcastSysTypeMsg<T> (T model, params string[] typeVal)
        {
            Type type = model.GetType();
            if (!BroadcastConfigCache.GetBroadcastConfig(type, out IRemoteBroadcast config))
            {
                throw new ErrorException("rpc.broadcast.config.error");
            }
            else if (!RpcClientHelper.ValidateData(config, model, out string error))
            {
                throw new ErrorException(error);
            }
            else
            {
                BroadcastCollect.GetBroadcast(config).BroadcastMsg<T>(config, model, typeVal);
            }
        }
        public static void BroadcastMsg<T> (T model, long rpcMerId, params string[] typeVal)
        {
            Type type = model.GetType();
            if (!BroadcastConfigCache.GetBroadcastConfig(type, out IRemoteBroadcast config))
            {
                throw new ErrorException("rpc.broadcast.config.error");
            }
            else if (!RpcClientHelper.ValidateData(config, model, out string error))
            {
                throw new ErrorException(error);
            }
            else
            {
                BroadcastCollect.GetBroadcast(config).BroadcastMsg<T>(config, model, rpcMerId, typeVal);
            }
        }
        /// <summary>
        /// 向所在服务组广播信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictate">指令</param>
        /// <param name="model">传输的实体</param>
        /// <param name="typeVal">接收的节点类型</param>
        /// <returns></returns>
        public static void BroadcastMsg<T> (string dictate, T model, params string[] typeVal)
        {
            if (string.IsNullOrEmpty(dictate))
            {
                throw new ErrorException("rpc.dictate.null");
            }
            Type type = model.GetType();
            if (!BroadcastConfigCache.GetBroadcastConfig(type, out IRemoteBroadcast config))
            {
                config = new IRemoteBroadcast(dictate, false, typeVal);
            }
            else
            {
                config = new IRemoteBroadcast(dictate, config, typeVal);
            }
            _InitBroadcast(config, type);
            _BroadcastMsg(config, model);
        }
        /// <summary>
        /// 向所在服务组广播信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictate"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static void BroadcastMsg<T> (string dictate, T model)
        {
            IRemoteBroadcast config = new(dictate, false, RpcStateCollect.LocalConfig.SystemType);
            _InitBroadcast(config, typeof(T));
            _BroadcastMsg(config, model);
        }
        private static void _InitBroadcast (IRemoteBroadcast config)
        {
            config.InitConfig(WebConfig.RpcConfig.MaxRetryNum);
            if (!config.RpcMerId.HasValue && !config.IsCrossGroup)
            {
                config.RpcMerId = RpcClient.CurrentSource.RpcMerId;
            }
        }
        private static void _InitBroadcast (IRemoteBroadcast config, Type type)
        {
            config.InitConfig(type, WebConfig.RpcConfig.MaxRetryNum);
            if (!config.RpcMerId.HasValue && !config.IsCrossGroup)
            {
                config.RpcMerId = RpcClient.CurrentSource.RpcMerId;
            }
        }
        public static void BroadcastGroupMsg<T> (string dictate, T model)
        {
            IRemoteBroadcast config = new(dictate, false, RpcStateCollect.LocalConfig.SysGroup);
            _InitBroadcast(config, typeof(T));
            _BroadcastMsg(config, model);
        }
        private static void _BroadcastMsg<T> (IRemoteBroadcast config, T model)
        {
            if (!RpcClientHelper.ValidateData(config, model, out string error))
            {
                throw new ErrorException(error);
            }
            else
            {
                BroadcastCollect.GetBroadcast(config).BroadcastMsg<T>(config, model);
            }
        }
        public static void BroadcastGroupMsg<T> (T model)
        {
            Type type = typeof(T);
            if (!RemoteConfigCache.GetRemoteConfig(type, out IRemoteConfig rConfig))
            {
                throw new ErrorException("rpc.config.error");
            }
            IRemoteBroadcast config = new(rConfig.SysDictate, false, RpcStateCollect.LocalConfig.SysGroup);
            _BroadcastMsg(config, model);
        }
        public static void BroadcastMsg<T> (T model, params long[] serverId)
        {
            Type type = model.GetType();
            if (!BroadcastConfigCache.GetBroadcastConfig(type, out IRemoteBroadcast config))
            {
                throw new ErrorException("rpc.broadcast.config.error");
            }
            else if (!RpcClientHelper.ValidateData(config, model, out string error))
            {
                throw new ErrorException(error);
            }
            BroadcastCollect.GetBroadcast(config).BroadcastMsg<T>(config, model, serverId);
        }

        public static void BroadcastMsg<T> (T model)
        {
            Type type = model.GetType();
            if (!BroadcastConfigCache.GetBroadcastConfig(type, out IRemoteBroadcast config))
            {
                throw new ErrorException("rpc.broadcast.config.error");
            }
            _BroadcastMsg(config, model);
        }
        public static void BroadcastMsg<T> (T model, BroadcastSet set)
        {
            Type type = model.GetType();
            if (!BroadcastConfigCache.GetBroadcastConfig(type, out IRemoteBroadcast config))
            {
                throw new ErrorException("rpc.broadcast.config.error");
            }
            IRemoteBroadcast data = config.Clone<IRemoteBroadcast>();
            data.ServerId = set.ServerId;
            data.RegionId = set.RegionId;
            data.RpcMerId = set.RpcMerId;
            data.TypeVal = set.SysType;
            if (!set.Dictate.IsNull())
            {
                data.SysDictate = set.Dictate;
            }
            if (set.IsOnly.HasValue)
            {
                data.IsOnly = set.IsOnly.Value;
            }
            if (set.IsCrossGroup.HasValue)
            {
                data.IsCrossGroup = set.IsCrossGroup.Value;
            }
            if (set.BroadcastType.HasValue)
            {
                data.BroadcastType = set.BroadcastType.Value;
            }
            if (set.IsExclude.HasValue)
            {
                data.IsExclude = set.IsExclude.Value;
            }
            _BroadcastMsg(config, model);
        }
        #endregion
    }
}