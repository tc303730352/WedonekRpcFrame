
using System;
using System.Reflection;

using RpcClient.Attr;
using RpcClient.Collect;
using RpcClient.Config;
using RpcClient.Interface;
using RpcClient.Model;
using RpcClient.Remote;
using RpcClient.Route;

using RpcModel;
using RpcModel.Tran.Model;

using RpcHelper;
namespace RpcClient.Helper
{
        internal class RpcClientHelper
        {
                public static IRemoteServer GetRemoteServer(BalancedType type, ServerConfig[] servers)
                {
                        if (servers.FindIndex(a => a.IsTransmit) != -1)
                        {
                                return new RangeRemoteHelper(type, servers);
                        }
                        else
                        {
                                return new BasicRemoteHelper(type, servers);
                        }
                }
                public static bool ValidateData<T>(IRemoteConfig config, T model, out string error)
                {
                        if (WebConfig.RpcConfig.IsValidateData && config.IsValidate)
                        {
                                return DataValidateHepler.ValidateData(model, out error);
                        }
                        error = null;
                        return true;
                }
                public static bool ValidateData<T>(IRemoteBroadcast config, T model, out string error)
                {
                        if (WebConfig.RpcConfig.IsValidateData && config.IsValidate)
                        {
                                return DataValidateHepler.ValidateData(model, out error);
                        }
                        error = null;
                        return true;
                }
                public static bool GetResult(IRemoteResult res, out string error)
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
                public static bool GetResult<Result>(IRemoteResult res, out Result result, out string error) where Result : IBasicRes
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
                public static TcpRemoteMsg GetQueueMsg<T>(IRemoteBroadcast config, T body)
                {
                        TcpRemoteMsg data = new TcpRemoteMsg
                        {
                                ExpireTime = WebConfig.RpcConfig.ExpireTime,
                                IsReply = false,
                                MsgBody = Tools.Json(body),
                                Source = RpcStateCollect.LocalSource
                        };
                        CurTranState tran = RpcTranCollect.Tran;
                        if (tran != null && tran.Level != TranLevel.RPC消息)
                        {
                                data.Tran = tran;
                        }
                        if (config.RemoteConfig != null)
                        {
                                IRemoteConfig rConfig = config.RemoteConfig;
                                data.IsSync = rConfig.IsSync;
                                data.LockType = rConfig.LockType;
                                data.LockId = rConfig.IsSync ? string.Join("_", config.SysDictate, rConfig.GetLockIdVal<T>(body)) : null;
                        }
                        data.Extend = RpcService.Service.LoadExtend(config.SysDictate);
                        return data;
                }
                public static TcpRemoteMsg GetQueueMsg(IRemoteBroadcast config, DynamicModel body)
                {
                        TcpRemoteMsg data = new TcpRemoteMsg
                        {
                                IsReply = false,
                                MsgBody = body.Json(),
                                Source = RpcStateCollect.LocalSource,
                                ExpireTime = WebConfig.RpcConfig.ExpireTime
                        };
                        CurTranState tran = RpcTranCollect.Tran;
                        if (tran != null && tran.Level != TranLevel.RPC消息)
                        {
                                data.Tran = tran;
                        }
                        if (config.RemoteConfig != null)
                        {
                                IRemoteConfig rConfig = config.RemoteConfig;
                                data.IsSync = rConfig.IsSync;
                                data.LockType = rConfig.LockType;
                                data.LockId = rConfig.IsSync ? string.Join("_", config.SysDictate, rConfig.GetLockIdVal(body)) : null;
                        }
                        data.Extend = RpcService.Service.LoadExtend(config.SysDictate);
                        return data;
                }
                public static TcpRemoteMsg GetRemoteMsg(DynamicModel body, IRemoteConfig config)
                {
                        TcpRemoteMsg msg = new TcpRemoteMsg
                        {
                                Source = RpcStateCollect.LocalSource,
                                MsgBody = body.Json(),
                                IsReply = config.IsReply,
                                IsSync = config.IsSync,
                                LockType = config.LockType,
                                LockId = config.IsSync ? string.Join("_", config.SysDictate, config.GetLockIdVal(body)) : null,
                                ExpireTime = WebConfig.RpcConfig.ExpireTime,
                                Tran = RpcTranCollect.Tran,
                                Extend = RpcService.Service.LoadExtend(config.SysDictate)
                        };
                        return msg;
                }
                public static TcpRemoteMsg GetTcpRemoteMsg<T>(string dictate, T body, IRemoteConfig config)
                {
                        TcpRemoteMsg msg = new TcpRemoteMsg
                        {
                                ExpireTime = WebConfig.RpcConfig.ExpireTime,
                                Source = RpcStateCollect.LocalSource,
                                MsgBody = Tools.Json(body),
                                IsReply = config.IsReply,
                                IsSync = config.IsSync,
                                LockType = config.LockType,
                                Tran = RpcTranCollect.Tran,
                                Extend = RpcService.Service.LoadExtend(config.SysDictate),
                                LockId = config.IsSync ? string.Join("_", dictate, config.GetLockIdVal<T>(body)) : null,
                        };
                        return msg;
                }
                public static TcpRemoteMsg GetDynamicTcpMsg(BroadcastDatum obj, MsgSource source, DynamicModel model, IRemoteConfig config)
                {
                        TcpRemoteMsg msg = new TcpRemoteMsg
                        {
                                Source = source,
                                MsgBody = obj.MsgBody,
                                IsReply = config.IsReply,
                                IsSync = config.IsSync,
                                LockType = config.LockType,
                                Tran = RpcTranCollect.Tran,
                                Extend = RpcService.Service.LoadExtend(config.SysDictate),
                                LockId = config.IsSync ? string.Join("_", config.SysDictate, config.GetLockIdVal(model)) : null
                        };
                        return msg;
                }
                public static TcpRemoteMsg GetTcpRemoteMsg(DynamicModel body, IRemoteConfig config)
                {
                        TcpRemoteMsg msg = new TcpRemoteMsg
                        {
                                ExpireTime = WebConfig.RpcConfig.ExpireTime,
                                Source = RpcStateCollect.LocalSource,
                                MsgBody = body.Json(),
                                IsReply = config.IsReply,
                                IsSync = config.IsSync,
                                LockType = config.LockType,
                                Tran = RpcTranCollect.Tran,
                                Extend = RpcService.Service.LoadExtend(config.SysDictate),
                                LockId = config.IsSync ? string.Join("_", config.SysDictate, config.GetLockIdVal(body)) : null
                        };
                        return msg;
                }
                public static TcpRemoteMsg GetTcpRemoteMsg<T>(T body, IRemoteConfig config)
                {
                        TcpRemoteMsg msg = new TcpRemoteMsg
                        {
                                ExpireTime = WebConfig.RpcConfig.ExpireTime,
                                Source = RpcStateCollect.LocalSource,
                                MsgBody = Tools.Json(body),
                                IsReply = config.IsReply,
                                IsSync = config.IsSync,
                                LockType = config.LockType,
                                Tran = RpcTranCollect.Tran,
                                Extend = RpcService.Service.LoadExtend(config.SysDictate),
                                LockId = config.IsSync ? string.Join("_", config.SysDictate, config.GetLockIdVal<T>(body)) : null
                        };
                        return msg;
                }
                public static bool CheckIsOutRoute(MethodInfo method)
                {
                        if (method.ReturnType != PublicDataDic.BoolType)
                        {
                                return false;
                        }
                        return method.GetParameters().IsExists(a => a.IsOut && a.Name == ConfigDic.ErrorParamName);
                }
                public static FuncParam GetParamType(ParameterInfo param)
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
                        else if (param.Name == ConfigDic.ErrorParamName && param.ParameterType.Name.StartsWith(PublicDataDic.StringTypeName))
                        {
                                return new FuncParam
                                {
                                        ParamType = FuncParamType.错误信息
                                };
                        }
                        else if (param.Name == ConfigDic.CountParamName && param.ParameterType.Name.StartsWith(PublicDataDic.LongTypeName))
                        {
                                return new FuncParam
                                {
                                        ParamType = FuncParamType.数据总数
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
                public static string GetRouteName(MethodInfo method)
                {
                        RpcTcpRouteAttr attr = method.GetCustomAttribute<RpcTcpRouteAttr>();
                        if (attr != null)
                        {
                                return attr.RouteName;
                        }
                        else
                        {
                                string name = method.Name;
                                if (name.EndsWith("Event"))
                                {
                                        return name.Substring(0, name.Length - 5);
                                }
                                return name;
                        }
                }
                public static bool InitParam(IMsg msg, FuncParam[] param, out object[] arg, bool isParam)
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
                                                arg[i] = RpcClient.Unity.Resolve(a.DataType);
                                        }
                                        else if (a.ParamType == FuncParamType.扩展参数)
                                        {
                                                if (msg.Extend != null && msg.Extend.TryGetValue(a.Key, out string val))
                                                {
                                                        arg[i] = Tools.StringParse(a.DataType, val);
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
