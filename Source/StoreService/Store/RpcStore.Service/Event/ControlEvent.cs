using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using RpcStore.RemoteModel.Control;
using RpcStore.RemoteModel.Control.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.Event
{
    /// <summary>
    /// 中控器节点
    /// </summary>
    internal class ControlEvent : IRpcApiService
    {
        private IRpcControlService _Service;

        public ControlEvent(IRpcControlService service)
        {
            _Service = service;
        }

        public int AddControl(AddControl add)
        {
            return _Service.Add(add.Datum);
        }

        public void DeleteControl(DeleteControl obj)
        {
            _Service.Delete(obj.Id);
        }

        public RpcControl GetControl(GetControl obj)
        {
            return _Service.Get(obj.Id);
        }

        public PagingResult<RpcControlData> QueryControl(QueryControl query)
        {
            return _Service.Query(query.ToBasicPage());
        }

        public void SetControl(SetControl obj)
        {
            _Service.Set(obj.Id, obj.Datum);
        }
    }
}
