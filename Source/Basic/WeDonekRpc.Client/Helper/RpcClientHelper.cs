
using System;
using System.Reflection;
using System.Threading;
using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Config;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Client.Remote;
using WeDonekRpc.Client.Route;
using WeDonekRpc.Client.TranService;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Json;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Helper
{
    internal class RpcClientHelper
    {
        private static uint _PageId = 0;
        public static IRemoteNode GetRemoteServer (BalancedType type, WeDonekRpc.Model.ServerConfig[] servers)
        {
            if (servers.FindIndex(a => a.IsTransmit) != -1)
            {
                return new RangeRemoteNode(type, servers);
            }
            else
            {
                return new BasicRemoteHelper(type, servers);
            }
        }
        public static bool ValidateData<T> (IRemoteConfig config, T model, out string error)
        {
            if (WebConfig.RpcConfig.IsValidateData && config.IsValidate)
            {
                return DataValidateHepler.ValidateData(model, out error);
            }
            error = null;
            return true;
        }
        public static bool ValidateData<T> (IRemoteBroadcast config, T model, out string error)
        {
            if (WebConfig.RpcConfig.IsValidateData && config.IsValidate)
            {
                return DataValidateHepler.ValidateData(model, out error);
            }
            error = null;
            return true;
        }
        public static bool GetResult (IRemoteResult res, out string error)
        {
            if (res.IsError)
            {
                error = res.ErrorMsg;
                return false;
            }
            else if (!res.IsReply)
            {
                error = null;
                return true;
            }
            else
            {
                BasicRes result = res.GetResult<BasicRes>();
                if (result == null)
                {
                    error = "rpc.data.return.null";
                    return false;
                }
                else if (result.IsError)
                {
                    error = result.ErrorMsg;
                    return false;
                }
                error = null;
                return true;
            }
        }
        public static BasicResult<Result> GetResult<Result> (IRemoteResult res)
        {
            if (res.IsError)
            {
                return new BasicResult<Result>(res.ErrorMsg, res.RemoteServerId);
            }
            BasicRes<Result> result = res.GetResult<BasicRes<Result>>();
            if (result == null)
            {
                return new BasicResult<Result>("rpc.data.return.null", res.RemoteServerId);
            }
            else if (result.IsError)
            {
                return new BasicResult<Result>(result.ErrorMsg, res.RemoteServerId);
            }
            return new BasicResult<Result>(result.Result, res.RemoteServerId);
        }
        public static bool GetResult<Result> (IRemoteResult res, out Result result, out string error) where Result : IBasicRes
        {
            if (res.IsError)
            {
                result = default;
                error = res.ErrorMsg;
                return false;
            }
            result = res.GetResult<Result>();
            if (result == null)
            {
                error = "rpc.data.return.null";
                return false;
            }
            else if (result.IsError)
            {
                error = result.ErrorMsg;
                return false;
            }
            error = null;
            return true;
        }
        public static TcpRemoteMsg GetQueueMsg<T> (IRemoteBroadcast config, T body)
        {
            TcpRemoteMsg data = new TcpRemoteMsg
            {
                ExpireTime = WebConfig.RpcConfig.ExpireTime,
                IsReply = false,
                MsgBody = JsonTools.Json(body, body.GetType()),
                Source = RpcStateCollect.LocalSource,
                PageId = Interlocked.Increment(ref _PageId)
            };
            if (config.RemoteConfig != null)
            {
                RemoteSet rConfig = config.RemoteConfig;
                data.IsSync = rConfig.IsEnableLock;
                data.LockType = rConfig.LockType;
                data.LockId = rConfig.IsEnableLock ? string.Join("_", config.SysDictate, rConfig.GetLockIdVal<T>(body)) : null;
            }
            data.Extend = RpcService.Service.LoadExtendEvent(config.SysDictate);
            return data;
        }
        public static TcpRemoteMsg GetQueueMsg (IRemoteBroadcast config, DynamicModel body)
        {
            TcpRemoteMsg data = new TcpRemoteMsg
            {
                IsReply = false,
                MsgBody = body.Json(),
                Source = RpcStateCollect.LocalSource,
                ExpireTime = WebConfig.RpcConfig.ExpireTime,
                PageId = Interlocked.Increment(ref _PageId)
            };
            if (config.RemoteConfig != null)
            {
                RemoteSet rConfig = config.RemoteConfig;
                data.IsSync = rConfig.IsEnableLock;
                data.LockType = rConfig.LockType;
                data.LockId = rConfig.IsEnableLock ? string.Join("_", config.SysDictate, rConfig.GetLockIdVal(body)) : null;
            }
            data.Extend = RpcService.Service.LoadExtendEvent(config.SysDictate);
            return data;
        }
        public static TcpRemoteMsg GetRemoteMsg (DynamicModel body, IRemoteConfig config)
        {
            return new TcpRemoteMsg
            {
                Source = RpcStateCollect.LocalSource,
                MsgBody = body?.Json(),
                IsReply = config.IsReply,
                IsSync = config.IsEnableLock,
                LockType = config.LockType,
                LockId = config.IsEnableLock ? string.Join("_", config.SysDictate, config.GetLockIdVal(body)) : null,
                ExpireTime = WebConfig.RpcConfig.ExpireTime,
                Tran = RpcTranService.GetTranSource(),
                Extend = RpcService.Service.LoadExtendEvent(config.SysDictate),
                PageId = Interlocked.Increment(ref _PageId)
            };
        }
        public static TcpRemoteMsg GetTcpRemoteMsg<T> (string dictate, T body, IRemoteConfig config)
        {
            return new TcpRemoteMsg
            {
                ExpireTime = WebConfig.RpcConfig.ExpireTime,
                Source = RpcStateCollect.LocalSource,
                MsgBody = JsonTools.Json(body, body.GetType()),
                IsReply = config.IsReply,
                IsSync = config.IsEnableLock,
                LockType = config.LockType,
                Tran = RpcTranService.GetTranSource(),
                Extend = RpcService.Service.LoadExtendEvent(config.SysDictate),
                LockId = config.IsEnableLock ? string.Join("_", dictate, config.GetLockIdVal<T>(body)) : null,
                PageId = Interlocked.Increment(ref _PageId)
            };
        }
        public static TcpRemoteMsg GetDynamicTcpMsg (BroadcastDatum obj, MsgSource source, DynamicModel model, IRemoteConfig config)
        {
            return new TcpRemoteMsg
            {
                Source = source,
                MsgBody = obj.MsgBody,
                IsReply = config.IsReply,
                IsSync = config.IsEnableLock,
                LockType = config.LockType,
                Tran = RpcTranService.GetTranSource(),
                Extend = RpcService.Service.LoadExtendEvent(config.SysDictate),
                LockId = config.IsEnableLock ? string.Join("_", config.SysDictate, config.GetLockIdVal(model)) : null,
                PageId = Interlocked.Increment(ref _PageId)
            };
        }
        public static TcpRemoteMsg GetTcpRemoteMsg (DynamicModel body, IRemoteConfig config)
        {
            return new TcpRemoteMsg
            {
                ExpireTime = WebConfig.RpcConfig.ExpireTime,
                Source = RpcStateCollect.LocalSource,
                MsgBody = body.Json(),
                IsReply = config.IsReply,
                IsSync = config.IsEnableLock,
                LockType = config.LockType,
                Tran = RpcTranService.GetTranSource(),
                Extend = RpcService.Service.LoadExtendEvent(config.SysDictate),
                LockId = config.IsEnableLock ? string.Join("_", config.SysDictate, config.GetLockIdVal(body)) : null,
                PageId = Interlocked.Increment(ref _PageId)
            };
        }
        public static TcpRemoteMsg GetTcpRemoteMsg<T> (T body, Type type, IRemoteConfig config)
        {
            return new TcpRemoteMsg
            {
                ExpireTime = WebConfig.RpcConfig.ExpireTime,
                Source = RpcStateCollect.LocalSource,
                MsgBody = JsonTools.Json(body, type),
                IsReply = config.IsReply,
                IsSync = config.IsEnableLock,
                LockType = config.LockType,
                Tran = RpcTranService.GetTranSource(),
                Extend = RpcService.Service.LoadExtendEvent(config.SysDictate),
                LockId = config.IsEnableLock ? string.Join("_", config.SysDictate, config.GetLockIdVal<T>(body)) : null,
                PageId = Interlocked.Increment(ref _PageId)
            };
        }
        public static TcpRemoteMsg GetTcpRemoteMsg<T> (T body, IRemoteConfig config, byte[] stream)
        {
            TcpRemoteMsg msg = new TcpRemoteMsg
            {
                ExpireTime = WebConfig.RpcConfig.ExpireTime,
                Stream = stream,
                Source = RpcStateCollect.LocalSource,
                MsgBody = JsonTools.Json(body, body.GetType()),
                IsReply = config.IsReply,
                IsSync = config.IsEnableLock,
                LockType = config.LockType,
                Tran = RpcTranService.GetTranSource(),
                Extend = RpcService.Service.LoadExtendEvent(config.SysDictate),
                LockId = config.IsEnableLock ? string.Join("_", config.SysDictate, config.GetLockIdVal<T>(body)) : null,
                PageId = Interlocked.Increment(ref _PageId)
            };
            return msg;
        }

        public static FuncParam GetParamType (ParameterInfo param)
        {
            if (!param.IsOut)
            {
                Type type = param.ParameterType;
                if (type.FullName == ConfigDic.MsgSourceType.FullName)
                {
                    return new FuncParam
                    {
                        ParamType = FuncParamType.数据源,
                        DataType = ConfigDic.MsgSourceType
                    };
                }
                else if (type.FullName == ConfigDic.ParamType.FullName)
                {
                    return new FuncParam
                    {
                        ParamType = FuncParamType.源,
                        DataType = ConfigDic.ParamType
                    };
                }
                else if (type.IsInterface)
                {
                    return new FuncParam
                    {
                        ParamType = FuncParamType.接口,
                        DataType = type
                    };
                }
                else if (type.IsArray && type.GetElementType().Name == PublicDataDic.ByteTypeName)
                {
                    return new FuncParam
                    {
                        ParamType = FuncParamType.数据流,
                        DataType = type
                    };
                }
                else if (Tools.IsBasicType(type))
                {
                    return new FuncParam
                    {
                        ParamType = FuncParamType.扩展参数,
                        DataType = type,
                        Key = param.Name
                    };
                }
                return new FuncParam
                {
                    ParamType = FuncParamType.参数,
                    DataType = type
                };
            }
            else
            {
                return new FuncParam
                {
                    ParamType = FuncParamType.返回值,
                    DataType = param.ParameterType.GetElementType()
                };
            }
        }

        public static bool InitParam (IMsg msg, FuncParam[] param, out object[] arg, bool isParam)
        {
            if (param.Length == 0)
            {
                arg = null;
                return true;
            }
            else if (isParam)
            {
                arg = new object[param.Length];
                for (int i = 0; i < param.Length; i++)
                {
                    FuncParam a = param[i];
                    if (a.ParamType == FuncParamType.数据源)
                    {
                        arg[i] = msg.Source;
                    }
                    else if (a.ParamType == FuncParamType.源)
                    {
                        arg[i] = msg;
                    }
                    else if (a.ParamType == FuncParamType.接口)
                    {
                        arg[i] = RpcClient.Ioc.Resolve(a.DataType);
                    }
                    else if (a.ParamType == FuncParamType.数据流)
                    {
                        arg[i] = msg.Stream;
                    }
                    else if (a.ParamType == FuncParamType.扩展参数)
                    {
                        if (msg.Extend != null && msg.Extend.TryGetValue(a.Key, out string val))
                        {
                            arg[i] = StringParseTools.Parse(val, a.DataType);
                        }
                    }
                    else if (a.ParamType == FuncParamType.参数)
                    {
                        object data = msg.GetMsgBody(a.DataType);
                        if (data == null)
                        {
                            return false;
                        }
                        arg[i] = data;
                    }
                }
            }
            else
            {
                arg = new object[param.Length];
            }
            return true;
        }



    }
}
