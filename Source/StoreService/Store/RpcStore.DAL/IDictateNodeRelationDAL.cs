namespace RpcStore.DAL
{
    public interface IDictateNodeRelationDAL
    {
        void Add(long subId, long[] parentId);
        void Clear(long subId);
        void Delete(long[] subId);
        long[] GetParents(long subId);
        long[] GetSubs(long parentId);
    }
}