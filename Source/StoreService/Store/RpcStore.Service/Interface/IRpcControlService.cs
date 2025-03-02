using WeDonekRpc.Client;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.Control.Model;

namespace RpcStore.Service.Interface
{
    public interface IRpcControlService
    {
        int Add(RpcControlDatum add);
        void Delete(int id);
        RpcControl Get(int id);
        PagingResult<RpcControlData> Query(IBasicPage paging);
        void Set(int id, RpcControlDatum set);
    }
}