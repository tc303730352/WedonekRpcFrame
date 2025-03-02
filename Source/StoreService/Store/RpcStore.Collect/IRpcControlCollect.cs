using WeDonekRpc.Model;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.Control.Model;

namespace RpcStore.Collect
{
    public interface IRpcControlCollect
    {
        int Add(RpcControlDatum add);
        void CheckIsRepeat(string ip, int port);
        void Delete(RpcControlModel control);
        RpcControlModel Get(int id);
        RpcControlModel[] Query(IBasicPage paging, out int count);
        bool Set(RpcControlModel control, RpcControlDatum set);
    }
}