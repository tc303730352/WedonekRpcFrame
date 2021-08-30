using RpcModel;

namespace RpcClient.Interface
{
        public interface IRemoteSend
        {
                IRemoteResult Send(IRemoteConfig config, DynamicModel obj);
                IRemoteResult Send(IRemoteConfig config, BroadcastDatum broadcast, MsgSource source);
                IRemoteResult Send(IRemoteConfig config, BroadcastDatum broadcast, long serverId, MsgSource source);
                IRemoteResult Send<T>(IRemoteConfig config, string dictate, string sysType, T model);
                IRemoteResult Send<T>(IRemoteConfig config, string sysType, T model);
                IRemoteResult Send<T>(IRemoteConfig config, long serverId, T model);
                IRemoteResult Send<T>(IRemoteConfig config, string dictate, long serverId, T model);
                IRemoteResult Send<T>(IRemoteConfig config, T model);
        }
}