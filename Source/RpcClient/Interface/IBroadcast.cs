using RpcModel;

namespace RpcClient.Interface
{
        internal interface IBroadcast
        {
                void BroadcastMsg(IRemoteBroadcast config, DynamicModel body);
                void BroadcastMsg<T>(IRemoteBroadcast config, T model, long[] serverId);
                void BroadcastMsg<T>(IRemoteBroadcast config, T model);
                void BroadcastMsg<T>(IRemoteBroadcast config, T model, string[] typeVal);
                void BroadcastMsg<T>(IRemoteBroadcast config, T model, long rpcMerId, string[] typeVal);
        }
}