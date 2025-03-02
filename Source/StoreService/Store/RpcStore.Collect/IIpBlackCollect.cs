using WeDonekRpc.Model;
using RpcStore.Model.ExtendDB;
using RpcStore.RemoteModel.IpBlack.Model;

namespace RpcStore.Collect
{
    public interface IIpBlackCollect
    {
        IpBlackListModel Add (IpBlackAdd datum);
        void Drop (IpBlackListModel black);
        IpBlackListModel Get (long id);
        IpBlackListModel[] Query (IpBlackQuery query, IBasicPage paging, out int count);
        bool Set (IpBlackListModel black, IpBlackSet set);
        void SetRemark (IpBlackListModel black, string remark);
    }
}