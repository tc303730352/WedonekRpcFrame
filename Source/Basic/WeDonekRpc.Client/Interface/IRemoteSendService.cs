using System.Collections.Generic;
using System.IO;
using WeDonekRpc.IOSendInterface;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Interface
{
    public interface IRemoteSendService
    {
        void BroadcastGroupMsg<T>(string dictate, T model);
        void BroadcastGroupMsg<T>(T model);
        void BroadcastMsg(IRemoteBroadcast config, Dictionary<string, object> body);
        void BroadcastMsg(IRemoteBroadcast config, DynamicModel body);
        void BroadcastMsg(string dictate, params long[] serverId);
        void BroadcastMsg<T>(IRemoteBroadcast config, T msg);

        bool BroadcastMsg<T>(IRemoteBroadcast config, T msg,out string error);
        void BroadcastMsg<T>(string dictate, T model);
        void BroadcastMsg<T>(string dictate, T model, params string[] typeVal);
        void BroadcastMsg<T>(T model);
        void BroadcastMsg<T>(T model, long rpcMerId, params string[] typeVal);
        void BroadcastMsg<T>(T model, params long[] serverId);
        void BroadcastSysTypeMsg<T>(T model, params string[] typeVal);
        void Send(IRemoteConfig config, Dictionary<string, object> dic);
        void Send(IRemoteConfig config, DynamicModel obj);
        void Send(long serverId, BroadcastDatum msg, MsgSource source);
        void Send(string sysType, BroadcastDatum msg, MsgSource source);
        Result Send<Result>(IRemoteConfig config);
        Result Send<T, Result>(long rpcMerId, int regionId, T model);
        Result Send<T, Result>(long serverId, T model);
        Result Send<T, Result>(T model);
        Result Send<T, Result>(T model, out long serverId);
        Result Send<T, Result>(T model, string sysType);
        void Send<T>(IRemoteConfig config, T data) where T : class;

        bool Send<T>(IRemoteConfig config, T data,out string error) where T : class;
        void Send<T>(long rpcMerId, int regionId, T data) where T : class;
        void Send<T>(long serverId, T data) where T : class;

        bool Send<T>(long serverId, T data,out string error) where T : class;
        void Send<T>(string sysType, T data);
        void Send<T>(T model);
        void Send<T>(T model, int regionId, long rpcMerId);
        IRemoteResult SendData(string sysType, BroadcastDatum msg);
        IUpTask SendFile<T>(T model, FileInfo file, UpFileAsync func, UpProgressAction progress);
        Result SendStream<T, Result>(T model, byte[] stream);
        void SendStream<T>(T model, byte[] stream);
    }
}