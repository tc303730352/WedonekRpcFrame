using RpcModel.Model;

namespace RpcClient.Interface
{
        public interface IRpcControlCollect
        {
                bool GetServer(int id, out RpcControlServer server, out string error);
                bool GetServers(out RpcControlServer[] servers, out string error);
        }
}