using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Interface
{
    internal interface IRemoteSend
    {
        /// <summary>
        /// 批量发送
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="config"></param>
        /// <param name="complete"></param>
        IRemoteResult[] MultipleSend<T> (T model, IRemoteConfig config);
        IRemoteResult Send (IRemoteConfig config, DynamicModel obj);
        IRemoteResult Send (IRemoteConfig config, BroadcastDatum broadcast, MsgSource source);
        IRemoteResult Send (IRemoteConfig config, BroadcastDatum broadcast, long serverId, MsgSource source);
        IRemoteResult Send<T> (IRemoteConfig config, string dictate, string sysType, T model);
        IRemoteResult Send<T> (IRemoteConfig config, string sysType, T model);
        IRemoteResult Send<T> (IRemoteConfig config, long serverId, T model);
        IRemoteResult Send<T> (IRemoteConfig config, string dictate, long serverId, T model);
        IRemoteResult Send<T> (IRemoteConfig config, T model);

        IRemoteResult SendStream<T> (IRemoteConfig config, T model, byte[] stream);
    }
}