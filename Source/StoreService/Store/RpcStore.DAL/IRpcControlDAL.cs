using WeDonekRpc.Model;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.Control.Model;

namespace RpcStore.DAL
{
    public interface IRpcControlDAL
    {
        int Add(RpcControlModel add);
        bool CheckIsRepeat(string ip, int port);
        void Delete(int id);
        RpcControlModel Get(int id);
        RpcControlModel[] Query(IBasicPage paging, out int count);
        void Set(int id, RpcControlDatum set);
    }
}