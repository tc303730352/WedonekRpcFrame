using RpcCentral.Model;
using RpcCentral.Model.DB;
using WeDonekRpc.Helper;
using WeDonekRpc.SqlSugar;

namespace RpcCentral.DAL.Repository
{
    internal class ServerTransmitConfigDAL : IServerTransmitConfigDAL
    {
        private readonly IRepository<ServerTransmitConfigModel> _Transmit;
        private readonly IRepository<ServerTransmitSchemeModel> _Scheme;

        public ServerTransmitConfigDAL (IRepository<ServerTransmitConfigModel> transmit,
            IRepository<ServerTransmitSchemeModel> scheme)
        {
            this._Transmit = transmit;
            this._Scheme = scheme;
        }

        public ServerTransmitScheme[] Gets (long systemType, long rpcMerId)
        {
            TransmitScheme[] schemes = this._Scheme.Gets<TransmitScheme>(a => a.SystemTypeId == systemType && a.RpcMerId == rpcMerId && a.IsEnable);
            if (schemes.IsNull())
            {
                return new ServerTransmitScheme[0];
            }
            ServerTransmitScheme[] list = schemes.ConvertAll(a => new ServerTransmitScheme
            {
                Id = a.Id,
                Scheme = a.Scheme,
                TransmitType = a.TransmitType,
                VerNum = a.VerNum
            });
            long[] ids = schemes.ConvertAll(a => a.Id);
            ServerTransmitConfigModel[] configs = this._Transmit.Gets(a => ids.Contains(a.SchemeId));
            list.ForEach(c =>
            {
                c.Transmit = configs.Convert(a => a.SchemeId == c.Id, a => new ServerTransmitConfig
                {
                    ServerCode = a.ServerCode,
                    TransmitConfig = a.TransmitConfig,
                });
            });
            return list;
        }
    }
}
