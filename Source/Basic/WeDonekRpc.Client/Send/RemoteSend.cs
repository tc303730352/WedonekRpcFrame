using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Helper;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Send
{
    [Attr.ClassLifetimeAttr(Attr.ClassLifetimeType.SingleInstance)]
    internal class RemoteSend : IRemoteSend
    {
        /// <summary>
        /// 多个发送
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="config"></param>
        /// <param name="complete"></param>
        public IRemoteResult[] MultipleSend<T> (T model, IRemoteConfig config)
        {
            IRemoteCursor cursor = RemoteServerNode.FindAllServer<T>(config.SystemType, config, model);
            if (cursor == null)
            {
                return null;
            }
            SendBody send;
            send.model = model;
            send.msg = RpcClientHelper.GetTcpRemoteMsg<T>(model, model.GetType(), config);
            send.dicate = config.SysDictate;
            send.config = config;
            IRemoteResult[] res = new IRemoteResult[cursor.Num];
            while (cursor.ReadNode(config.IsProhibitTrace, out IRemote remote))
            {
                if (!remote.IsUsable)
                {
                    res[cursor.Current] = new IRemoteResult(remote.ServerId, "rpc.server.no.usable");
                }
                else
                {
                    send.remote = remote;
                    res[cursor.Current] = _Send(ref send, 0);
                }
            }
            return res;
        }
        public IRemoteResult Send (IRemoteConfig config, DynamicModel model)
        {
            if (!RemoteServerNode.FindServer(config.SystemType, config, model, out IRemote remote))
            {
                return new IRemoteResult("rpc.no.server");
            }
            else
            {
                SendBody send;
                send.model = model;
                send.msg = RpcClientHelper.GetRemoteMsg(model, config);
                send.dicate = config.SysDictate;
                send.config = config;
                send.remote = remote;
                return _SendByT<dynamic>(ref send, 0);
            }
        }
        public IRemoteResult SendStream<T> (IRemoteConfig config, T model, byte[] stream)
        {
            if (!RemoteServerNode.FindServer<T>(config.SystemType, config, model, out IRemote remote))
            {
                return new IRemoteResult("rpc.no.server");
            }
            else
            {
                SendBody send;
                send.model = model;
                send.msg = RpcClientHelper.GetTcpRemoteMsg<T>(model, config, stream);
                send.dicate = config.SysDictate;
                send.config = config;
                send.remote = remote;
                return _SendByT<T>(ref send, 0);
            }
        }

        public IRemoteResult Send<T> (IRemoteConfig config, T model)
        {
            if (!RemoteServerNode.FindServer<T>(config.SystemType, config, model, out IRemote remote))
            {
                return new IRemoteResult("rpc.no.server");
            }
            else
            {
                SendBody send;
                send.model = model;
                send.msg = RpcClientHelper.GetTcpRemoteMsg<T>(model, model.GetType(), config);
                send.dicate = config.SysDictate;
                send.config = config;
                send.remote = remote;
                return _SendByT<T>(ref send, 0);
            }
        }
        public IRemoteResult Send<T> (IRemoteConfig config, string sysType, T model)
        {
            if (!RemoteServerNode.FindServer<T>(sysType, config, model, out IRemote remote))
            {
                return new IRemoteResult("rpc.no.server");
            }
            else
            {
                SendBody send;
                send.model = model;
                send.msg = RpcClientHelper.GetTcpRemoteMsg<T>(model, model.GetType(), config);
                send.dicate = config.SysDictate;
                send.config = config;
                send.remote = remote;
                return _SendByT<T>(ref send, 0);
            }
        }
        public IRemoteResult Send<T> (IRemoteConfig config, string dictate, string sysType, T model)
        {
            if (!RemoteServerNode.FindServer<T>(sysType, config, model, out IRemote remote))
            {
                return new IRemoteResult("rpc.no.server");
            }
            else
            {
                if (string.IsNullOrEmpty(dictate))
                {
                    dictate = config.SysDictate;
                }
                SendBody send;
                send.model = model;
                send.msg = RpcClientHelper.GetTcpRemoteMsg<T>(dictate, model, config);
                send.dicate = dictate;
                send.config = config;
                send.remote = remote;
                return _SendByT<T>(ref send, 0);
            }
        }
        public IRemoteResult Send<T> (IRemoteConfig config, long serverId, T model)
        {
            if (!RemoteServerCollect.GetUsableServer(serverId, config.IsProhibitTrace, out IRemote remote))
            {
                return _GetServerNoUsable(serverId);
            }
            else
            {
                SendBody send;
                send.model = model;
                send.msg = RpcClientHelper.GetTcpRemoteMsg<T>(model, model.GetType(), config);
                send.dicate = config.SysDictate;
                send.config = config;
                send.remote = remote;
                return _Send(ref send, 0);
            }
        }
        public IRemoteResult Send<T> (IRemoteConfig config, string dictate, long serverId, T model)
        {
            if (!RemoteServerCollect.GetUsableServer(serverId, config.IsProhibitTrace, out IRemote remote))
            {
                return _GetServerNoUsable(serverId);
            }
            else
            {
                if (string.IsNullOrEmpty(dictate))
                {
                    dictate = config.SysDictate;
                }
                SendBody send;
                send.model = model;
                send.msg = RpcClientHelper.GetTcpRemoteMsg<T>(dictate, model, config);
                send.dicate = dictate;
                send.config = config;
                send.remote = remote;
                return _Send(ref send, 0);
            }
        }
        public IRemoteResult Send (IRemoteConfig config, BroadcastDatum broadcast, MsgSource source)
        {
            if (source == null)
            {
                source = RpcStateCollect.LocalSource;
            }
            DynamicModel model = new DynamicModel(broadcast.MsgBody);
            if (!RemoteServerNode.FindServer(config.SystemType, config, model, out IRemote remote))
            {
                return new IRemoteResult("rpc.no.server");
            }
            else
            {
                SendBody send;
                send.model = model;
                send.msg = RpcClientHelper.GetDynamicTcpMsg(broadcast, source, model, config);
                send.dicate = config.SysDictate;
                send.config = config;
                send.remote = remote;
                return _SendByT<dynamic>(ref send, 0);
            }
        }
        public IRemoteResult Send (IRemoteConfig config, BroadcastDatum broadcast, long serverId, MsgSource source)
        {
            if (source == null)
            {
                source = RpcStateCollect.LocalSource;
            }
            if (!RemoteServerCollect.GetUsableServer(serverId, config.IsProhibitTrace, out IRemote remote))
            {
                return _GetServerNoUsable(serverId);
            }
            else
            {
                DynamicModel model = new DynamicModel(broadcast.MsgBody);
                SendBody send;
                send.model = model;
                send.msg = RpcClientHelper.GetDynamicTcpMsg(broadcast, source, model, config);
                send.dicate = config.SysDictate;
                send.config = config;
                send.remote = remote;
                return _Send(ref send, 0);
            }
        }

        #region 私有方法

        private static IRemoteResult _Send (ref SendBody send, int retryNum)
        {
            RpcClient.SendEvent(ref send, retryNum);
            IRemoteResult result = send.Send(retryNum);
            RpcClient.SendEnd(ref send, result);
            if (result.IsError && send.CheckIsRetry(retryNum, result.ErrorMsg))
            {
                return _Send(ref send, retryNum + 1);
            }
            return result;
        }
        private static IRemoteResult _SendByT<T> (ref SendBody send, int retryNum)
        {
            RpcClient.SendEvent(ref send, retryNum);
            IRemoteResult result = send.Send(retryNum);
            RpcClient.SendEnd(ref send, result);
            if (result.IsError && send.CheckIsRetry(retryNum, result.ErrorMsg))
            {
                if (!RemoteServerNode.FindServer<T>(send.config.SystemType, send.config, (T)send.model, out IRemote remote))
                {
                    return new IRemoteResult("rpc.no.server");
                }
                send.remote = remote;
                return _SendByT<T>(ref send, retryNum + 1);
            }
            return result;
        }

        private static IRemoteResult _GetServerNoUsable (long serverId)
        {
            return new IRemoteResult(string.Format("rpc.server.no.usable[{0}]", serverId));
        }
        #endregion
    }
}