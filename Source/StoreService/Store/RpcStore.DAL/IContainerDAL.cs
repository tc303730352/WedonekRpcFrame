namespace RpcStore.DAL
{
    public interface IContainerDAL
    {
        Dictionary<long, string> GetInternalAddr (long[] ids);
        void Clear (long groupId);
        void Delete (long id);
        string GetInternalAddr (long id);
    }
}