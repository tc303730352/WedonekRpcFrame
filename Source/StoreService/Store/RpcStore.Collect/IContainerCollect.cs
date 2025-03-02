namespace RpcStore.Collect
{
    public interface IContainerCollect
    {
        Dictionary<long, string> GetInternalAddr (long[] ids);
        string GetInternalAddr (long id);
        void Clear (long groupId);
    }
}