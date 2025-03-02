using System.Collections.Generic;
using System.IO;
using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.IOSendInterface;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client
{
    [Attr.ClassLifetimeAttr(Attr.ClassLifetimeType.SingleInstance)]
    internal class RpcSendClient : IRpcSendClient
    {
        #region 发送数据
        public bool Send<T> (long serverId, T data, out string error) where T : class
        {
            return RemoteCollect.Send(serverId, data, out error);
        }
        public bool Send<T> (long rpcMerId, int regionId, T data, out string error) where T : class
        {
            return RemoteCollect.Send(rpcMerId, regionId, data, out error);
        }

        public bool Send (IRemoteConfig config, Dictionary<string, object> dic, out string error)
        {
            return RemoteCollect.Send(config, dic, out error);
        }
        public bool Send (IRemoteConfig config, DynamicModel obj, out string error)
        {
            return RemoteCollect.Send(config, obj, out error);
        }
        public bool Send<T> (IRemoteConfig config, T data, out string error) where T : class
        {
            return RemoteCollect.Send(config, data, out error);
        }

        public bool Send<T> (string sysType, T data, out string error)
        {
            return RemoteCollect.Send(sysType, data, out error);
        }

        public bool Send<T, Result> (T model, out Result result, out string error)
        {
            return RemoteCollect.Send(model, out result, out error);
        }
        public bool Send<T, Result> (long rpcMerId, int regionId, T model, out Result result, out string error)
        {
            return RemoteCollect.Send(rpcMerId, regionId, model, out result, out error);
        }
        public bool Send<T, Result> (T model, out Result result, out long serverId, out string error)
        {
            return RemoteCollect.Send(model, out result, out serverId, out error);
        }
        public bool Send<T, Result> (T model, out Result[] result, out long count, out string error)
        {
            return RemoteCollect.Send(model, out result, out count, out error);
        }

        public bool Send<T, Result> (T model, string sysType, out Result result, out string error)
        {
            return RemoteCollect.Send(model, sysType, out result, out error);
        }
        public bool Send<T, Result> (long serverId, T model, out Result result, out string error)
        {
            return RemoteCollect.Send(serverId, model, out result, out error);
        }
        /// <summary>
        /// 发送数据并返回结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Send<T> (T model, out string error)
        {
            return RemoteCollect.Send(model, out error);
        }

        public bool Send<T> (T model, int regionId, long rpcMerId, out string error)
        {
            return RemoteCollect.Send(model, regionId, rpcMerId, out error);
        }

        public bool SendFile<T> (T model, FileInfo file, UpFileAsync func, UpProgressAction progress, out IUpTask upTask, out string error)
        {
            return RemoteCollect.SendFile(model, file, func, progress, out upTask, out error);
        }
        #endregion


        #region 消息广播


        public void BroadcastMsg (string dictate, params long[] serverId)
        {
            RemoteCollect.BroadcastMsg(dictate, serverId);
        }
        public void BroadcastMsg (IRemoteBroadcast config, Dictionary<string, object> body)
        {
            RemoteCollect.BroadcastMsg(config, body);
        }
        public void BroadcastMsg (IRemoteBroadcast config, DynamicModel body)
        {
            RemoteCollect.BroadcastMsg(config, body);
        }
        public void BroadcastSysTypeMsg<T> (T model, params string[] typeVal)
        {
            RemoteCollect.BroadcastSysTypeMsg(model, typeVal);
        }
        public void BroadcastMsg<T> (T model, long rpcMerId, params string[] typeVal)
        {
            RemoteCollect.BroadcastMsg(model, rpcMerId, typeVal);
        }
        /// <summary>
        /// 向所在服务组广播信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictate"></param>
        /// <param name="model"></param>
        /// <param name="typeVal"></param>
        /// <returns></returns>
        public void BroadcastMsg<T> (string dictate, T model, params string[] typeVal)
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
        public void BroadcastMsg<T> (string dictate, T model)
        {
            RemoteCollect.BroadcastMsg(dictate, model);
        }

        public void BroadcastGroupMsg<T> (string dictate, T model)
        {
            RemoteCollect.BroadcastGroupMsg(dictate, model);
        }

        public void BroadcastGroupMsg<T> (T model)
        {
            RemoteCollect.BroadcastGroupMsg(model);
        }
        public void BroadcastMsg<T> (T model, params long[] serverId)
        {
            RemoteCollect.BroadcastMsg(model, serverId);
        }

        public void BroadcastMsg<T> (T model)
        {
            RemoteCollect.BroadcastMsg(model);
        }

        public void BroadcastMsg<T> (IRemoteBroadcast config, T msg)
        {
            RemoteCollect.BroadcastMsg(config, msg);
        }

        #endregion
    }
}
