using System.Collections.Generic;
using System.IO;
using WeDonekRpc.IOSendInterface;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Interface
{
    public interface IRpcSendClient
    {
        void BroadcastGroupMsg<T>(string dictate, T model);
        void BroadcastGroupMsg<T>(T model);
        void BroadcastMsg(IRemoteBroadcast config, Dictionary<string, object> body);
        void BroadcastMsg(IRemoteBroadcast config, DynamicModel body);
        void BroadcastMsg<T>(IRemoteBroadcast config, T msg);
        void BroadcastMsg(string dictate, params long[] serverId);
        void BroadcastMsg<T>(string dictate, T model);
        void BroadcastMsg<T>(string dictate, T model, params string[] typeVal);
        void BroadcastMsg<T>(T model);
        void BroadcastMsg<T>(T model, long rpcMerId, params string[] typeVal);
        void BroadcastMsg<T>(T model, params long[] serverId);
        void BroadcastSysTypeMsg<T>(T model, params string[] typeVal);
        bool Send(IRemoteConfig config, Dictionary<string, object> dic, out string error);
        bool Send(IRemoteConfig config, DynamicModel obj, out string error);
        bool Send<T, Result>(long rpcMerId, int regionId, T model, out Result result, out string error);
        bool Send<T, Result>(long serverId, T model, out Result result, out string error);
        bool Send<T, Result>(T model, out Result result, out long serverId, out string error);
        bool Send<T, Result>(T model, out Result result, out string error);
        bool Send<T, Result>(T model, out Result[] result, out long count, out string error);
        bool Send<T, Result>(T model, string sysType, out Result result, out string error);
        bool Send<T>(IRemoteConfig config, T data, out string error) where T : class;
        bool Send<T>(long rpcMerId, int regionId, T data, out string error) where T : class;
        bool Send<T>(long serverId, T data, out string error) where T : class;
        bool Send<T>(string sysType, T data, out string error);
        bool Send<T>(T model, int regionId, long rpcMerId, out string error);
        bool Send<T>(T model, out string error);
        bool SendFile<T>(T model, FileInfo file, UpFileAsync func, UpProgressAction progress, out IUpTask upTask, out string error);
    }
}