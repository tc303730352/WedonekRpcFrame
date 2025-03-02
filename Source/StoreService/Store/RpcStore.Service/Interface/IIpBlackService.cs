using WeDonekRpc.Client;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.IpBlack.Model;

namespace RpcStore.Service.Interface
{
    public interface IIpBlackService
    {
        long Add (IpBlackAdd add);
        void Drop (long id);
        IpBlackDatum Get (long id);
        PagingResult<IpBlack> Query (IpBlackQuery query, IBasicPage paging);
        void Set (long id, IpBlackSet set);
        void SetRemark (long id, string remark);
    }
}