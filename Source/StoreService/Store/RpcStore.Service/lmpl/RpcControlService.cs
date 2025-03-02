using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using RpcManageClient;
using WeDonekRpc.Model;
using RpcStore.Collect;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.Control.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.lmpl
{
    internal class RpcControlService : IRpcControlService
    {
        private readonly IRpcControlCollect _RpcControl;
        private IServerRegionCollect _Region;
        private IRpcServerCollect _RpcServer;
        public RpcControlService(IRpcControlCollect control,
            IRpcServerCollect rpcServer,
            IServerRegionCollect region)
        {
            _RpcServer = rpcServer;
            _RpcControl = control;
            _Region = region;
        }

        public int Add(RpcControlDatum add)
        {
            int id = _RpcControl.Add(add);
            _RpcServer.RefreshControl(add.RegionId);
            return id;
        }

        public void Delete(int id)
        {
            RpcControlModel control = _RpcControl.Get(id);
            _RpcControl.Delete(control);
            _RpcServer.RefreshControl(control.RegionId);
        }

        public RpcControl Get(int id)
        {
            RpcControlModel control = _RpcControl.Get(id);
            return control.ConvertMap<RpcControlModel, RpcControl>();
        }

        public PagingResult<RpcControlData> Query(IBasicPage paging)
        {
            RpcControlModel[] list = _RpcControl.Query(paging, out int count);
            if (list.IsNull())
            {
                return new PagingResult<RpcControlData>();
            }
            Dictionary<int, string> regions = _Region.GetNames(list.Distinct(a => a.RegionId));
            RpcControlData[] datas = list.ConvertMap<RpcControlModel, RpcControlData>((a, b) =>
            {
                b.RegionName = regions.GetValueOrDefault(a.RegionId);
                return b;
            });
            return new PagingResult<RpcControlData>(count, datas);
        }

        public void Set(int id, RpcControlDatum set)
        {
            RpcControlModel control = _RpcControl.Get(id);
            if (_RpcControl.Set(control, set))
            {
                _RpcServer.RefreshControl(control.RegionId);
            }
        }
    }
}
