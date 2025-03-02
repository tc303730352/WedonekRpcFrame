using System.Collections.Generic;
using System.Drawing;
using System.IO;
using WeDonekRpc.IOSendInterface;
using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Send
{
    [Attr.ClassLifetimeAttr(Attr.ClassLifetimeType.SingleInstance)]
    internal class RemoteSendService : IRemoteSendService
    {
        #region 发送数据
        public void Send<T>(long serverId, T data) where T : class
        {
            if (!RemoteCollect.Send<T>(serverId, data, out string error))
            {
                throw new ErrorException(error);
            }
        }
        public bool Send<T>(long serverId, T data, out string error) where T : class
        {
            return RemoteCollect.Send<T>(serverId, data, out error);
        }
        public void Send<T>(long rpcMerId, int regionId, T data) where T : class
        {
            if (!RemoteCollect.Send<T>(rpcMerId, regionId, data, out string error))
            {
                throw new ErrorException(error);
            }
        }

        public void Send(IRemoteConfig config, Dictionary<string, object> dic)
        {
            if (!RemoteCollect.Send(config, dic, out string error))
            {
                throw new ErrorException(error);
            }
        }
        public void Send(IRemoteConfig config, DynamicModel obj)
        {
            if (!RemoteCollect.Send(config, obj, out string error))
            {
                throw new ErrorException(error);
            }
        }
        public void Send<T>(IRemoteConfig config, T data) where T : class
        {
            if (!RemoteCollect.Send<T>(config, data, out string error))
            {
                throw new ErrorException(error);
            }
        }
        public bool Send<T>(IRemoteConfig config, T data, out string error) where T : class
        {
            return RemoteCollect.Send<T>(config, data, out error);
        }
        public Result Send<Result>(IRemoteConfig config)
        {
            if (!RemoteCollect.Send<Result>(config, out Result result, out string error))
            {
                throw new ErrorException(error);
            }
            return result;
        }
        public void Send<T>(string sysType, T data)
        {
            if (!RemoteCollect.Send<T>(sysType, data, out string error))
            {
                throw new ErrorException(error);
            }
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
        public Result Send<T, Result>(T model)
        {
            if (!RemoteCollect.Send<T, Result>(model, out Result result, out string error))
            {
                throw new ErrorException(error);
            }
            return result;
        }

        public Result Send<T, Result>(long rpcMerId, int regionId, T model)
        {
            if (!RemoteCollect.Send<T, Result>(rpcMerId, regionId, model, out Result result, out string error))
            {
                throw new ErrorException(error);
            }
            return result;
        }
        public Result Send<T, Result>(T model, out long serverId)
        {
            if (!RemoteCollect.Send<T, Result>(model, out Result result, out serverId, out string error))
            {
                throw new ErrorException(error);
            }
            return result;
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
        public Result Send<T, Result>(T model, string sysType)
        {
            if (!RemoteCollect.Send<T, Result>(model, sysType, out Result result, out string error))
            {
                throw new ErrorException(error);
            }
            return result;
        }
        public Result Send<T, Result>(long serverId, T model)
        {
            if (!RemoteCollect.Send<T, Result>(serverId, model, out Result result, out string error))
            {
                throw new ErrorException(error);
            }
            return result;
        }
        /// <summary>
        /// 发送数据并返回结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public void Send<T>(T model)
        {
            if (!RemoteCollect.Send<T>(model, out string error))
            {
                throw new ErrorException(error);
            }
        }

        public void Send<T>(T model, int regionId, long rpcMerId)
        {
            if (!RemoteCollect.Send<T>(model, regionId, rpcMerId, out string error))
            {
                throw new ErrorException(error);
            }
        }

        public IUpTask SendFile<T>(T model, FileInfo file, UpFileAsync func, UpProgressAction progress)
        {
            if (!RemoteCollect.SendFile<T>(model, file, func, progress, out IUpTask upTask, out string error))
            {
                throw new ErrorException(error);
            }
            return upTask;
        }

        public void Send(long serverId, BroadcastDatum msg, MsgSource source)
        {
            if (!RemoteCollect.Send(serverId, msg, source, out string error))
            {
                throw new ErrorException(error);
            }
        }

        public IRemoteResult SendData(string sysType, BroadcastDatum msg)
        {
            return RemoteCollect.SendData(sysType, msg);
        }
        public void Send(string sysType, BroadcastDatum msg, MsgSource source)
        {
            if (!RemoteCollect.Send(sysType, msg, source, out string error))
            {
                throw new ErrorException(error);
            }
        }

        public Result SendStream<T, Result>(T model, byte[] stream)
        {
            if (!RemoteCollect.SendStream(model, stream, out Result result, out string error))
            {
                throw new ErrorException(error);
            }
            return result;
        }
        public void SendStream<T>(T model, byte[] stream)
        {
            if (!RemoteCollect.SendStream(model, stream, out string error))
            {
                throw new ErrorException(error);
            }
        }
        #endregion



        #region 消息广播


        public void BroadcastMsg(string dictate, params long[] serverId)
        {
            RemoteCollect.BroadcastMsg(dictate, serverId);
        }
        public void BroadcastMsg(IRemoteBroadcast config, Dictionary<string, object> body)
        {
            RemoteCollect.BroadcastMsg(config, body);
        }
        public void BroadcastMsg(IRemoteBroadcast config, DynamicModel body)
        {
            RemoteCollect.BroadcastMsg(config, body);
        }
        public void BroadcastMsg<T>(IRemoteBroadcast config, T msg)
        {
            RemoteCollect.BroadcastMsg(config, msg);
        }
        public bool BroadcastMsg<T>(IRemoteBroadcast config, T msg,out string error)
        {
            return RemoteCollect.BroadcastMsg<T>(config, msg, out error);
        }
        public void BroadcastSysTypeMsg<T>(T model, params string[] typeVal)
        {
            RemoteCollect.BroadcastSysTypeMsg(model, typeVal);
        }
        public void BroadcastMsg<T>(T model, long rpcMerId, params string[] typeVal)
        {
            RemoteCollect.BroadcastMsg(model, rpcMerId, typeVal);
        }
        /// <summary>
        /// 向所在服务组广播信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictate">指令</param>
        /// <param name="model">传输的实体</param>
        /// <param name="typeVal">接收的节点类型</param>
        /// <returns></returns>
        public void BroadcastMsg<T>(string dictate, T model, params string[] typeVal)
        {
            RemoteCollect.BroadcastMsg(dictate, model, typeVal);
        }
        /// <summary>
        /// 向所在服务组广播信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictate"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public void BroadcastMsg<T>(string dictate, T model)
        {
            RemoteCollect.BroadcastMsg(dictate, model);
        }

        public void BroadcastGroupMsg<T>(string dictate, T model)
        {
            RemoteCollect.BroadcastGroupMsg(dictate, model);
        }

        public void BroadcastGroupMsg<T>(T model)
        {
            RemoteCollect.BroadcastGroupMsg(model);
        }
        public void BroadcastMsg<T>(T model, params long[] serverId)
        {
            RemoteCollect.BroadcastMsg(model, serverId);
        }

        public void BroadcastMsg<T>(T model)
        {
            RemoteCollect.BroadcastMsg(model);
        }

        #endregion
    }
}
