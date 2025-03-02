using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using RpcStore.DAL;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.Environment;
using RpcStore.RemoteModel.Environment.Model;

namespace RpcStore.Service.Event
{
    internal class EnvironmentEvent : IRpcApiService
    {
        private readonly IServerEnvironmentDAL _Service;

        public EnvironmentEvent (IServerEnvironmentDAL service)
        {
            this._Service = service;
        }

        public ServerEnvironment GetEnvironment (GetEnvironment obj)
        {
            ServerEnvironmentModel model = this._Service.Get(obj.ServerId);
            return model.ConvertMap<ServerEnvironmentModel, ServerEnvironment>();
        }
    }
}
