using RpcStore.RemoteModel.TransmitScheme;
using RpcStore.RemoteModel.TransmitScheme.Model;
using RpcStore.Service.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;

namespace RpcStore.Service.Event
{
    internal class TransmitSchemeEvent : IRpcApiService
    {
        private readonly IServerTransmitSchemeService _Service;

        public TransmitSchemeEvent (IServerTransmitSchemeService service)
        {
            this._Service = service;
        }
        public void SetTransmitSchemeIsEnable (SetTransmitSchemeIsEnable obj)
        {
            this._Service.SetIsEnable(obj.Id, obj.IsEnable);
        }
        public TransmitSchemeDetailed GetTransmitDetailed (GetTransmitDetailed obj)
        {
            return this._Service.GetDetailed(obj.Id);
        }
        public void SyncTransmitItem (SyncTransmitItem obj)
        {
            this._Service.SyncItem(obj.SchemeId, obj.Transmits);
        }
        public void DeleteTransmitScheme (DeleteTransmitScheme obj)
        {
            this._Service.Delete(obj.Id);
        }
        public TransmitSchemeData GetTransmitScheme (GetTransmitScheme obj)
        {
            return this._Service.Get(obj.Id);
        }
        public void SetTransmitScheme (SetTransmitScheme obj)
        {
            this._Service.SetScheme(obj.Id, obj.Datum);
        }
        public PagingResult<TransmitScheme> QueryTransmitScheme (QueryTransmitScheme obj)
        {
            return this._Service.Query(obj.Query, obj.ToBasicPage());
        }
        public long AddTransmitScheme (AddTransmitScheme obj)
        {
            return this._Service.Add(obj.Datum);
        }
    }
}
