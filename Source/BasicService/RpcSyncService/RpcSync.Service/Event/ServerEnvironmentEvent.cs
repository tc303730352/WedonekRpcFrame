using RpcSync.Collect;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Model;
using WeDonekRpc.Model.Server;
namespace RpcSync.Service.Event
{
    internal class ServerEnvironmentEvent : IRpcApiService
    {
        private readonly IServerEnvironmentCollect _Service;

        public ServerEnvironmentEvent (IServerEnvironmentCollect service)
        {
            this._Service = service;
        }
        public void UploadLoadModule (UploadLoadModule obj, MsgSource source)
        {
            this._Service.SetModules(source.ServerId, obj.Modules);
        }
        public void UploadEnvironment (UploadEnvironment obj, MsgSource source)
        {
            this._Service.Sync(source.ServerId, obj.Environment);
        }
    }
}
