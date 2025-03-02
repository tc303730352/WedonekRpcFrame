using RpcManageClient;
using RpcStore.Collect;
using RpcStore.Model.DB;
using RpcStore.Model.ServerConfig;
using RpcStore.RemoteModel.ServerType.Model;
using RpcStore.RemoteModel.ServerVer.Model;
using RpcStore.Service.Interface;
using WeDonekRpc.Helper;

namespace RpcStore.Service.lmpl
{
    internal class RpcMerServerVerService : IRpcMerServerVerService
    {
        private readonly IRemoteGroupCollect _Group;
        private readonly IRpcMerServerVerCollect _ServerVer;
        private readonly IServerCollect _Server;
        private readonly IServerTypeCollect _ServerType;
        private readonly IRpcServerCollect _RpcServer;
        public RpcMerServerVerService (IRemoteGroupCollect group,
            IRpcMerServerVerCollect serverVer,
            IServerCollect server,
            IRpcServerCollect rpcServer,
            IServerTypeCollect serverType)
        {
            this._RpcServer = rpcServer;
            this._Group = group;
            this._ServerVer = serverVer;
            this._Server = server;
            this._ServerType = serverType;
        }
        public void SetCurrentVer (long rpcMerId, long sysTypeId, int verNum)
        {
            RpcMerServerVerModel ver = this._ServerVer.Find(rpcMerId, sysTypeId);
            if (ver == null)
            {
                this._ServerVer.Add(rpcMerId, sysTypeId, verNum);
            }
            else
            {
                this._ServerVer.SetCurrentVer(ver, verNum);
            }
            this._RpcServer.RefreshVerNum(rpcMerId, sysTypeId, verNum);
        }
        public ServerVerInfo[] GetVerList (long rpcMerId)
        {
            long[] serverId = this._Group.GetServerId(rpcMerId, true);
            if (serverId.IsNull())
            {
                return null;
            }
            SystemTypeVerNum[] vers = this._Server.GetAllVerNums(serverId);
            ServerType[] types = this._ServerType.Gets(vers.ConvertAll(a => a.SystemTypeId));
            Dictionary<long, int> curVers = this._ServerVer.GetVers(rpcMerId);
            return types.ConvertAll(c =>
            {
                int cur = curVers.GetValueOrDefault(c.Id, 0);
                return new ServerVerInfo
                {
                    SystemTypeId = c.Id,
                    TypeName = c.SystemName,
                    CurrentVer = cur.FormatVerNum(),
                    LastVerNum = vers.Convert(a => a.SystemTypeId == c.Id && a.VerNum != cur, a => a.VerNum.FormatVerNum())
                };
            });
        }
    }
}
