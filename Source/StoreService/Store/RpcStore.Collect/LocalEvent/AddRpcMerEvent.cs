using RpcStore.DAL;
using RpcStore.Model.ServerConfig;
using RpcStore.Model.ServerGroup;
using RpcStore.RemoteModel.ServerConfig.Model;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace RpcStore.Collect.LocalEvent
{
    [LocalEventName("Add")]
    internal class AddRpcMerEvent : IEventHandler<Model.RpcMerEvent>
    {
        private readonly IRemoteServerConfigDAL _Server;
        private readonly IRemoteServerGroupDAL _ServerGroup;

        public AddRpcMerEvent (IRemoteServerConfigDAL server,
            IRemoteServerGroupDAL serverGroup)
        {
            this._Server = server;
            this._ServerGroup = serverGroup;
        }

        public void HandleEvent (Model.RpcMerEvent data, string eventName)
        {
            BasicService[] items = this._Server.GetBasics(new ServerConfigQuery
            {
                SystemTypeId = new long[]
                {
                    1,
                    7
                },
                ServiceState = new RpcServiceState[]
                {
                    RpcServiceState.正常
                }
            });
            this._ServerGroup.Adds(data.RpcMer.Id, items.ConvertAll(c => new RemoteServerGroup
            {
                ServerId = c.Id,
                RegionId = c.RegionId,
                ServiceType = c.ServiceType,
                SystemType = c.SystemType,
                TypeVal = c.SystemType == 1 ? "sys.sync" : "sys.extend"
            }));
        }
    }
}
