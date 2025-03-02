using RpcManageClient;
using RpcStore.Collect;
using RpcStore.Model.DB;
using RpcStore.Model.TransmitScheme;
using RpcStore.RemoteModel.TransmitScheme.Model;
using RpcStore.Service.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace RpcStore.Service.lmpl
{
    internal class ServerTransmitSchemeService : IServerTransmitSchemeService
    {
        private readonly IServerTransmitSchemeCollect _Service;
        private readonly IServerTypeCollect _ServerType;
        private readonly IRpcServerCollect _RpcServer;

        public ServerTransmitSchemeService (IServerTransmitSchemeCollect service,
            IServerTypeCollect serverType,
            IRpcServerCollect rpcServer)
        {
            this._Service = service;
            this._ServerType = serverType;
            this._RpcServer = rpcServer;
        }

        public void SetIsEnable (long id, bool isEnable)
        {
            ServerTransmitSchemeModel scheme = this._Service.Get(id);
            if (this._Service.SetIsEnable(scheme, isEnable))
            {
                this._RpcServer.RefreshTransmit(scheme.RpcMerId, scheme.SystemTypeId);
            }
        }
        public void Delete (long id)
        {
            ServerTransmitSchemeModel scheme = this._Service.Get(id);
            this._Service.Delete(scheme);
        }
        public TransmitSchemeDetailed GetDetailed (long id)
        {
            ServerTransmitScheme scheme = this._Service.GetScheme(id);
            TransmitSchemeDetailed detailed = scheme.ConvertMap<ServerTransmitScheme, TransmitSchemeDetailed>();
            detailed.SystemType = this._ServerType.GetName(detailed.SystemTypeId);
            detailed.VerNumStr = detailed.VerNum.FormatVerNum();
            return detailed;
        }
        public TransmitSchemeData Get (long id)
        {
            ServerTransmitSchemeModel scheme = this._Service.Get(id);
            return scheme.ConvertMap<ServerTransmitSchemeModel, TransmitSchemeData>();
        }
        public void SetScheme (long id, TransmitSchemeSet set)
        {
            ServerTransmitSchemeModel scheme = this._Service.Get(id);
            this._Service.SetScheme(scheme, set);
        }
        public PagingResult<TransmitScheme> Query (TransmitSchemeQuery query, IBasicPage paging)
        {
            ServerTransmitSchemeModel[] list = this._Service.Query(query, paging, out int count);
            if (list.IsNull())
            {
                return new PagingResult<TransmitScheme>(count);
            }
            Dictionary<long, string> types = this._ServerType.GetNames(list.ConvertAll(a => a.SystemTypeId));
            TransmitScheme[] schemes = list.ConvertAll(c =>
            {
                return new TransmitScheme
                {
                    SystemType = types.GetValueOrDefault(c.SystemTypeId),
                    SystemTypeId = c.SystemTypeId,
                    Scheme = c.Scheme,
                    Show = c.Show,
                    VerNum = c.VerNum.FormatVerNum(),
                    AddTime = c.AddTime,
                    Id = c.Id,
                    IsEnable = c.IsEnable,
                    TransmitType = c.TransmitType
                };
            });
            return new PagingResult<TransmitScheme>(schemes, count);
        }

        public long Add (TransmitSchemeAdd datum)
        {
            return this._Service.Add(datum);
        }

        public void SyncItem (long schemeId, TransmitDatum[] transmits)
        {
            this._Service.SyncItem(schemeId, transmits);
        }
    }
}
