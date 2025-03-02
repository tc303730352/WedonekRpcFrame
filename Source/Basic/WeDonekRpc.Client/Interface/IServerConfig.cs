using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Interface
{
        public interface IServerConfig
        {
                short ConfigPrower { get; }
                long GroupId { get; }
                string GroupTypeVal { get; }
                string Name { get; }
                int RegionId { get; }
                long ServerId { get; }
                string ServerIp { get; }
                int ServerPort { get; }
                RpcServiceState ServiceState { get; }
                long SystemType { get; }
                long VerNum { get; }
        }
}