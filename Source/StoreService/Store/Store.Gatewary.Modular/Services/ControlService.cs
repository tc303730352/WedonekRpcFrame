using WeDonekRpc.Model;
using RpcStore.RemoteModel.Control;
using RpcStore.RemoteModel.Control.Model;
using Store.Gatewary.Modular.Interface;

namespace Store.Gatewary.Modular.Services
{
    internal class ControlService : IControlService
    {
        public int AddControl(RpcControlDatum datum)
        {
            return new AddControl
            {
                Datum = datum,
            }.Send();
        }

        public void DeleteControl(int id)
        {
            new DeleteControl
            {
                Id = id,
            }.Send();
        }

        public RpcControl GetControl(int id)
        {
            return new GetControl
            {
                Id = id,
            }.Send();
        }

        public RpcControlData[] QueryControl(IBasicPage paging, out int count)
        {
            return new QueryControl
            {
                Index = paging.Index,
                Size = paging.Size,
                NextId = paging.NextId,
                SortName = paging.SortName,
                IsDesc = paging.IsDesc
            }.Send(out count);
        }

        public void SetControl(int id, RpcControlDatum datum)
        {
            new SetControl
            {
                Id = id,
                Datum = datum,
            }.Send();
        }

    }
}
