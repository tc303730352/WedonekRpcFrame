using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

using RpcClient.Config;
using RpcClient.Helper;
using RpcClient.Interface;

using RpcHelper;

using RpcModel;

using SocketTcpClient.UpFile;

namespace RpcClient.Collect
{
        public class RemoteCollect
        {
                private static readonly IRemoteSend _Remote = null;
                private static readonly ConcurrentDictionary<string, IRemoteConfig> _RemoteConfig = new ConcurrentDictionary<string, IRemoteConfig>();
                private static readonly ConcurrentDictionary<string, IRemoteBroadcast> _BroadcastConfig = new ConcurrentDictionary<string, IRemoteBroadcast>();

                static RemoteCollect()
                {
                        _Remote = new Send.RemoteSend();
                }

                #region 私有方法


                private static bool _GetRemoteConfig(Type type, out IRemoteConfig config)
                {
                        string name = type.FullName;
                        if (_RemoteConfig.TryGetValue(name, out config))
                        {
                                return true;
                        }
                        object[] datas = type.GetCustomAttributes(ConfigDic.RemoteConfigType, true);
                        if (datas == null || datas.Length == 0)
                        {
                                return false;
                        }
                        config = _RemoteConfig.GetOrAdd(name, (IRemoteConfig)datas[0]);
                        config.InitConfig(type);
                        return true;
                }

                private static bool _GetBroadcastConfig(Type type, out IRemoteBroadcast config)
                {
                        string name = type.FullName;
                        if (_BroadcastConfig.TryGetValue(name, out config))
                        {
                                return true;
                        }
                        object[] datas = type.GetCustomAttributes(ConfigDic.BroadcastType, true);
                        if (datas == null || datas.Length == 0)
                        {
                                return false;
                        }
                        IRemoteBroadcast obj = (IRemoteBroadcast)datas[0];
                        obj.InitConfig(type);
                        if (_GetRemoteConfig(type, out IRemoteConfig rConfig))
                        {
                                obj.RemoteConfig = rConfig;
                        }
                        if (obj.RpcMerId == 0)
                        {
                                obj.RpcMerId = RpcClient.CurrentSource.RpcMerId;
                        }
                        if (obj.RegionId == -1)
                        {
                                obj.RegionId = RpcClient.CurrentSource.RegionId;
                        }
                        config = _BroadcastConfig.GetOrAdd(name, obj);
                        return true;
                }
                #endregion

                #region 发送数据
                public static bool Send<T>(long serverId, T data, out string error) where T : class
                {
                        IRemoteResult result = _Send<T>(serverId, data);
                        return RpcClientHelper.GetResult(result, out error);
                }
                public static bool Send<T>(long rpcMerId, int regionId, T data, out string error) where T : class
                {
                        IRemoteResult result = _Send<T>(data, regionId, rpcMerId);
                        return RpcClientHelper.GetResult(result, out error);
                }

                public static bool Send(IRemoteConfig config, Dictionary<string, object> dic, out string error)
                {
                        return Send(config, new DynamicModel(dic), out error);
                }
                public static bool Send(IRemoteConfig config, DynamicModel obj, out string error)
                {
                        config.InitConfig();
                        IRemoteResult result = _Remote.Send(config, obj);
                        return RpcClientHelper.GetResult(result, out error);
                }
                public static bool Send<T>(IRemoteConfig config, T data, out string error) where T : class
                {
                        IRemoteResult result = _Remote.Send(config, data);
                        return RpcClientHelper.GetResult(result, out error);
                }

                public static bool Send<T>(string sysType, T data, out string error)
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
                public static bool Send<T, Result>(T model, out Result result, out string error)
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
                public static bool Send<T, Result>(long rpcMerId, int regionId, T model, out Result result, out string error)
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
                public static bool Send<T, Result>(T model, out Result result, out long serverId, out string error)
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
                public static bool Send<T, Result>(T model, out Result[] result, out long count, out string error)
                {
                        if (!_Send(model, out BasicResPaging<Result> res, out error))
                        {
                                count = 0;
                                result = null;
                                return false;
                        }
                        else
                        {
                                result = res.DataList;
                                count = res.Count;
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
                public static bool Send<T, Result>(T model, string sysType, out Result result, out string error)
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
                public static bool Send<T, Result>(long serverId, T model, out Result result, out string error)
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
                public static bool Send<T>(T model, out string error)
                {
                        IRemoteResult res = _Send<T>(model);
                        return RpcClientHelper.GetResult(res, out error);
                }

                public static bool Send<T>(T model, int regionId, long rpcMerId, out string error)
                {
                        IRemoteResult res = _Send<T>(model, regionId, rpcMerId);
                        return RpcClientHelper.GetResult(res, out error);
                }

                public static bool SendFile<T>(T model, FileInfo file, UpFileAsync func, UpProgress progress, out UpTask upTask, out string error)
                {
                        Type type = model.GetType();
                        if (!_GetRemoteConfig(type, out IRemoteConfig config))
                        {
                                upTask = null;
                                error = "public.class.config.error";
                                return false;
                        }
                        else if (!RpcClientHelper.ValidateData(config, model, out error))
                        {
                                upTask = null;
                                return false;
                        }
                        else if (!RemoteServerHelper.FindServer<T>(config.SystemType, config, model, out IRemote remote))
                        {
                                upTask = null;
                                error = "public.no.server";
                                return false;
                        }
                        else
                        {
                                TcpRemoteMsg msg = RpcClientHelper.GetTcpRemoteMsg<T>(model, config);
                                return remote.SendFile(config, msg, file, func, progress, out upTask, out error);
                        }
                }

                public static bool Send(long serverId, BroadcastDatum msg, MsgSource source, out string error)
                {
                        return _Send(new IRemoteConfig(msg), msg, serverId, source, out error);
                }

                public static IRemoteResult SendData(string sysType, BroadcastDatum msg)
                {
                        return _Send(sysType, msg, RpcStateCollect.LocalSource);
                }
                public static bool Send(string sysType, BroadcastDatum msg, MsgSource source, out string error)
                {
                        IRemoteResult result = _Send(sysType, msg, source);
                        return RpcClientHelper.GetResult(result, out error);
                }
                #endregion

                #region 发送的方法
                private static bool _Send<T, Result>(long serverId, T model, out Result result, out string error) where Result : IBasicRes
                {
                        IRemoteResult res = _Send<T>(serverId, model);
                        return RpcClientHelper.GetResult(res, out result, out error);
                }


                private static IRemoteResult _Send<T>(T model, int regionId, long rpcMerId)
                {
                        Type type = model.GetType();
                        if (!_GetRemoteConfig(type, out IRemoteConfig config))
                        {
                                return new IRemoteResult("public.class.config.error");
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
                private static IRemoteResult _Send<T>(T model)
                {
                        Type type = model.GetType();
                        if (!_GetRemoteConfig(type, out IRemoteConfig config))
                        {
                                return new IRemoteResult("public.class.config.error");
                        }
                        else if (!RpcClientHelper.ValidateData(config, model, out string error))
                        {
                                return new IRemoteResult(error);
                        }
                        else
                        {
                                return _Remote.Send(config, model);
                        }
                }

                private static IRemoteResult _Send<T>(long serverId, T model)
                {
                        Type type = model.GetType();
                        if (!_GetRemoteConfig(type, out IRemoteConfig config))
                        {
                                return new IRemoteResult("public.class.config.error");
                        }
                        else if (!RpcClientHelper.ValidateData(config, model, out string error))
                        {
                                return new IRemoteResult(error);
                        }
                        else
                        {
                                return _Remote.Send(config, serverId, model);
                        }
                }

                private static IRemoteResult _Send<T>(T model, string sysType)
                {
                        Type type = model.GetType();
                        if (!_GetRemoteConfig(type, out IRemoteConfig config))
                        {
                                return new IRemoteResult("public.class.config.error");
                        }
                        else if (!RpcClientHelper.ValidateData(config, model, out string error))
                        {
                                return new IRemoteResult(error);
                        }
                        else
                        {
                                return _Remote.Send(config, sysType, model);
                        }
                }
                private static bool _Send<T, Result>(T model, int regionId, long rpcMerId, out Result result, out string error) where Result : IBasicRes
                {
                        IRemoteResult res = _Send<T>(model, regionId, rpcMerId);
                        return RpcClientHelper.GetResult(res, out result, out error);
                }
                private static bool _Send<T, Result>(T model, out Result result, out string error) where Result : IBasicRes
                {
                        IRemoteResult res = _Send<T>(model);
                        return RpcClientHelper.GetResult(res, out result, out error);
                }
                private static bool _Send<T, Result>(T model, out Result result, out long serverId, out string error) where Result : IBasicRes
                {
                        IRemoteResult res = _Send(model);
                        serverId = res.RemoteServerId;
                        return RpcClientHelper.GetResult(res, out result, out error);
                }
                private static IRemoteResult _Send(string sysType, BroadcastDatum msg, MsgSource source)
                {
                        IRemoteConfig config = new IRemoteConfig(msg, sysType, source.SourceServerId);
                        return _Remote.Send(config, msg, source);
                }
                private static bool _Send(IRemoteConfig config, BroadcastDatum broadcast, long serverId, MsgSource source, out string error)
                {
                        IRemoteResult result = _Remote.Send(config, broadcast, serverId, source);
                        if (result.IsError)
                        {
                                error = result.ErrorMsg;
                                return false;
                        }
                        error = null;
                        return true;
                }
                #endregion

                #region 消息广播


                public static void BroadcastMsg(string dictate, params long[] serverId)
                {
                        if (string.IsNullOrEmpty(dictate))
                        {
                                throw new ErrorException("rpc.dictate.null");
                        }
                        IRemoteBroadcast config = new IRemoteBroadcast(dictate, false, serverId)
                        {
                                BroadcastType = BroadcastType.默认,
                                IsCrossGroup = true,
                                IsOnly = false,
                                RpcMerId = RpcClient.CurrentSource.RpcMerId
                        };
                        BroadcastCollect.GetBroadcast(config).BroadcastMsg(config, string.Empty);
                }
                public static void BroadcastMsg(IRemoteBroadcast config, Dictionary<string, object> body)
                {
                        BroadcastMsg(config, new DynamicModel(body));
                }
                public static void BroadcastMsg(IRemoteBroadcast config, DynamicModel body)
                {
                        _InitBroadcast(config);
                        BroadcastCollect.GetBroadcast(config).BroadcastMsg(config, body);
                }
                public static void BroadcastSysTypeMsg<T>(T model, params string[] typeVal)
                {
                        Type type = model.GetType();
                        if (!_GetBroadcastConfig(type, out IRemoteBroadcast config))
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
                public static void BroadcastMsg<T>(T model, long rpcMerId, params string[] typeVal)
                {
                        Type type = model.GetType();
                        if (!_GetBroadcastConfig(type, out IRemoteBroadcast config))
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
                /// <param name="dictate"></param>
                /// <param name="data"></param>
                /// <returns></returns>
                public static void BroadcastMsg<T>(string dictate, T model, params string[] typeVal)
                {
                        if (string.IsNullOrEmpty(dictate))
                        {
                                throw new ErrorException("rpc.dictate.null");
                        }
                        Type type = model.GetType();
                        if (!_GetBroadcastConfig(type, out IRemoteBroadcast config))
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
                /// <param name="data"></param>
                /// <returns></returns>
                public static void BroadcastMsg<T>(string dictate, T model)
                {
                        IRemoteBroadcast config = new IRemoteBroadcast(dictate, false, RpcStateCollect.LocalSource.SystemType);
                        _InitBroadcast(config, typeof(T));
                        _BroadcastMsg(config, model);
                }
                private static void _InitBroadcast(IRemoteBroadcast config)
                {
                        config.InitConfig();
                        if (config.RpcMerId == 0)
                        {
                                config.RpcMerId = RpcClient.CurrentSource.RpcMerId;
                        }
                }
                private static void _InitBroadcast(IRemoteBroadcast config, Type type)
                {
                        config.InitConfig(type);
                        if (config.RpcMerId == 0)
                        {
                                config.RpcMerId = RpcClient.CurrentSource.RpcMerId;
                        }
                }
                public static void BroadcastGroupMsg<T>(string dictate, T model)
                {
                        IRemoteBroadcast config = new IRemoteBroadcast(dictate, false, RpcStateCollect.LocalSource.GroupTypeVal);
                        _InitBroadcast(config, typeof(T));
                        _BroadcastMsg(config, model);
                }
                private static void _BroadcastMsg<T>(IRemoteBroadcast config, T model)
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
                public static void BroadcastGroupMsg<T>(T model)
                {
                        Type type = typeof(T);
                        if (!_GetRemoteConfig(type, out IRemoteConfig rConfig))
                        {
                                throw new ErrorException("rpc.config.error");
                        }
                        IRemoteBroadcast config = new IRemoteBroadcast(rConfig.SysDictate, false, RpcStateCollect.LocalSource.GroupTypeVal);
                        _BroadcastMsg(config, model);
                }
                public static void BroadcastMsg<T>(T model, params long[] serverId)
                {
                        Type type = model.GetType();
                        if (!_GetBroadcastConfig(type, out IRemoteBroadcast config))
                        {
                                throw new ErrorException("rpc.broadcast.config.error");
                        }
                        else if (!RpcClientHelper.ValidateData(config, model, out string error))
                        {
                                throw new ErrorException(error);
                        }
                        BroadcastCollect.GetBroadcast(config).BroadcastMsg<T>(config, model, serverId);
                }

                public static void BroadcastMsg<T>(T model)
                {
                        Type type = model.GetType();
                        if (!_GetBroadcastConfig(type, out IRemoteBroadcast config))
                        {
                                throw new ErrorException("rpc.broadcast.config.error");
                        }
                        _BroadcastMsg(config, model);
                }

                #endregion
        }
}