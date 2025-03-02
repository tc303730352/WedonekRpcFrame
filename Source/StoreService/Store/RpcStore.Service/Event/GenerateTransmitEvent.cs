using RpcStore.Collect;
using RpcStore.RemoteModel.TransmitScheme;
using RpcStore.RemoteModel.TransmitScheme.Model;
using WeDonekRpc.Client.Interface;

namespace RpcStore.Service.Event
{
    internal class GenerateTransmitEvent : IRpcApiService
    {
        private readonly ITransmitGenerateService _Service;

        public GenerateTransmitEvent (ITransmitGenerateService service)
        {
            this._Service = service;
        }

        public TransmitDatum[] GenerateTransmit (GenerateTransmit obj)
        {
            return this._Service.GetTransmits(obj.Arg);
        }
    }
}
