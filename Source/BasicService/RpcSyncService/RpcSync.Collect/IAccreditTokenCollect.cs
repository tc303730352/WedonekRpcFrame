using RpcSync.Model;
using RpcSync.Model.DB;

namespace RpcSync.Collect
{
    public interface IAccreditTokenCollect
    {
        void Add(AccreditTokenModel add);
        bool Delete(string id);
        AccreditTokenDatum Get(string id);
        string[] GetSubId(string accreditId);
        bool Set(string accreditId, AccreditTokenSet set);
        void SetOverTime(string accreditId, DateTime overTime);
    }
}