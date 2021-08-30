using RpcClient.Collect;
using RpcClient.Config;
using RpcClient.Helper;
using RpcClient.Interface;

using RpcModel;

namespace RpcClient.Send
{
        internal class RemoteSend : IRemoteSend
        {
                public IRemoteResult Send(IRemoteConfig config, DynamicModel obj)
                {
                        if (!RemoteServerHelper.FindServer(config.SystemType, config, obj, out IRemote remote))
                        {
                                return _GetNoServer(config);
                        }
                        else
                        {
                                TcpRemoteMsg msg = RpcClientHelper.GetRemoteMsg(obj, config);
                                return _Send(config, msg, obj, remote, 0);
                        }
                }
                public IRemoteResult Send<T>(IRemoteConfig config, T model)
                {
                        if (!RemoteServerHelper.FindServer<T>(config.SystemType, config, model, out IRemote remote))
                        {
                                return _GetNoServer(config);
                        }
                        else
                        {
                                TcpRemoteMsg msg = RpcClientHelper.GetTcpRemoteMsg<T>(model, config);
                                return _Send<T>(config, msg, model, remote, 0);
                        }
                }
                public IRemoteResult Send<T>(IRemoteConfig config, string sysType, T model)
                {
                        if (!RemoteServerHelper.FindServer<T>(sysType, config, model, out IRemote remote))
                        {
                                return _GetNoServer(config, sysType);
                        }
                        else
                        {
                                TcpRemoteMsg msg = RpcClientHelper.GetTcpRemoteMsg<T>(model, config);
                                return _Send<T>(config, msg, model, remote, 0);
                        }
                }
                public IRemoteResult Send<T>(IRemoteConfig config, string dictate, string sysType, T model)
                {
                        if (!RemoteServerHelper.FindServer<T>(sysType, config, model, out IRemote remote))
                        {
                                return _GetNoServer(config, sysType);
                        }
                        else
                        {
                                if (string.IsNullOrEmpty(dictate))
                                {
                                        dictate = config.SysDictate;
                                }
                                TcpRemoteMsg msg = RpcClientHelper.GetTcpRemoteMsg<T>(dictate, model, config);
                                return _Send<T>(dictate, config, msg, model, remote, 0);
                        }
                }
                public IRemoteResult Send<T>(IRemoteConfig config, long serverId, T model)
                {
                        if (!RemoteServerCollect.GetRemoteServer(serverId, out IRemote remote))
                        {
                                return _GetServerNoUsable(serverId);
                        }
                        else
                        {
                                TcpRemoteMsg msg = RpcClientHelper.GetTcpRemoteMsg<T>(model, config);
                                return _Send<T>(config, msg, model, remote, 0);
                        }
                }
                public IRemoteResult Send<T>(IRemoteConfig config, string dictate, long serverId, T model)
                {
                        if (!RemoteServerCollect.GetRemoteServer(serverId, out IRemote remote))
                        {
                                return _GetServerNoUsable(serverId);
                        }
                        else
                        {
                                if (string.IsNullOrEmpty(dictate))
                                {
                                        dictate = config.SysDictate;
                                }
                                TcpRemoteMsg msg = RpcClientHelper.GetTcpRemoteMsg<T>(dictate, model, config);
                                return _Send(config, dictate, msg, remote, 0);
                        }
                }
                public IRemoteResult Send(IRemoteConfig config, BroadcastDatum broadcast, MsgSource source)
                {
                        if (source == null)
                        {
                                source = RpcStateCollect.LocalSource;
                        }
                        DynamicModel model = new DynamicModel(broadcast.MsgBody);
                        if (!RemoteServerHelper.FindServer(config.SystemType, config, model, out IRemote remote))
                        {
                                return _GetNoServer(config);
                        }
                        else
                        {
                                TcpRemoteMsg msg = RpcClientHelper.GetDynamicTcpMsg(broadcast, source, model, config);
                                return _Send<dynamic>(config, msg, model, remote, 0);
                        }
                }
                public IRemoteResult Send(IRemoteConfig config, BroadcastDatum broadcast, long serverId, MsgSource source)
                {
                        if (source == null)
                        {
                                source = RpcStateCollect.LocalSource;
                        }
                        if (!RemoteServerCollect.GetRemoteServer(serverId, out IRemote remote))
                        {
                                return _GetServerNoUsable(serverId);
                        }
                        else
                        {
                                DynamicModel model = new DynamicModel(broadcast.MsgBody);
                                TcpRemoteMsg msg = RpcClientHelper.GetDynamicTcpMsg(broadcast, source, model, config);
                                return _Send<dynamic>(config, msg, model, remote, 0);
                        }
                }

                #region 私有方法

                private static IRemoteResult _Send(IRemoteConfig config, string dicate, TcpRemoteMsg msg, IRemote remote, int retryNum)
                {
                        if (remote.SendData(dicate, config, msg, out TcpRemoteReply reply, out string error))
                        {
                                return new IRemoteResult(remote.ServerId, reply, config.IsReply);
                        }
                        else if (!config.IsAllowRetry || retryNum >= WebConfig.RpcConfig.MaxRetryNum)
                        {
                                return new IRemoteResult(remote.ServerId, error);
                        }
                        else
                        {
                                ++retryNum;
                                return _Send(config, dicate, msg, remote, retryNum);
                        }
                }

                private static IRemoteResult _Send<T>(IRemoteConfig config, TcpRemoteMsg msg, T model, IRemote remote, int retryNum)
                {
                        if (remote.SendData(config, msg, out TcpRemoteReply reply, out string error))
                        {
                                return new IRemoteResult(remote.ServerId, reply, config.IsReply);
                        }
                        else if (!config.IsAllowRetry || retryNum >= WebConfig.RpcConfig.MaxRetryNum || config.SystemType == null)
                        {
                                return new IRemoteResult(remote.ServerId, error);
                        }
                        else
                        {
                                ++retryNum;
                                return !RemoteServerHelper.FindServer<T>(config.SystemType, config, model, out remote)
                                        ? _GetNoServer(config)
                                        : _Send<T>(config, msg, model, remote, retryNum);
                        }
                }
                private static IRemoteResult _Send<T>(string dicate, IRemoteConfig config, TcpRemoteMsg msg, T model, IRemote remote, int retryNum)
                {
                        if (remote.SendData(dicate, config, msg, out TcpRemoteReply reply, out string error))
                        {
                                return new IRemoteResult(remote.ServerId, reply, config.IsReply);
                        }
                        else if (!config.IsAllowRetry || retryNum >= WebConfig.RpcConfig.MaxRetryNum)
                        {
                                return new IRemoteResult(remote.ServerId, error);
                        }
                        else
                        {
                                ++retryNum;
                                return !RemoteServerHelper.FindServer<T>(config.SystemType, config, model, out remote)
                                        ? _GetNoServer(config)
                                        : _Send<T>(dicate, config, msg, model, remote, retryNum);
                        }
                }

                private static IRemoteResult _GetNoServer(IRemoteConfig config)
                {
                        return new IRemoteResult(string.Format("public.no.server[{0},{1}]", config.SystemType, config.SysDictate));
                }
                private static IRemoteResult _GetNoServer(IRemoteConfig config, string sysType)
                {
                        return new IRemoteResult(string.Format("public.no.server[{0},{1}]", sysType, config.SysDictate));
                }
                private static IRemoteResult _GetServerNoUsable(long serverId)
                {
                        return new IRemoteResult(string.Format("rpc.server.no.usable[{0}]", serverId));
                }
                #endregion
        }
}