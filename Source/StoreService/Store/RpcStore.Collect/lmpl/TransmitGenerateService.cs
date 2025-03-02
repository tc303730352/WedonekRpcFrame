using RpcStore.Collect.Helper;
using RpcStore.Collect.Model;
using RpcStore.DAL;
using RpcStore.Model.ServerConfig;
using RpcStore.RemoteModel.TransmitScheme.Model;
using WeDonekRpc.Helper;

namespace RpcStore.Collect.lmpl
{
    internal class TransmitGenerateService : ITransmitGenerateService
    {
        private readonly IRemoteServerGroupDAL _ServerGroup;
        private readonly IRemoteServerConfigDAL _Server;

        public TransmitGenerateService (IRemoteServerGroupDAL serverGroup, IRemoteServerConfigDAL server)
        {
            this._ServerGroup = serverGroup;
            this._Server = server;
        }

        public TransmitDatum[] GetTransmits (TransmitGenerate generate)
        {
            long[] serverId = this._ServerGroup.GetServerId(generate.RpcMerId, generate.SystemTypeId);
            if (serverId.IsNull())
            {
                return null;
            }
            ServerConfigDatum[] list = this._Server.GetServices(serverId);
            list = list.FindAll(a => a.VerNum == generate.VerNum);
            if (list.IsNull())
            {
                return null;
            }
            TransmitGenerateHelper helper = new TransmitGenerateHelper();
            Transmit[] trans = list.ConvertAll(a => new Transmit
            {
                ServerCode = a.ServerCode,
                ServerName = a.ServerName,
                Range = []
            });
            return helper.InitTransmits(trans, generate.TransmitType);
        }

    }
}
