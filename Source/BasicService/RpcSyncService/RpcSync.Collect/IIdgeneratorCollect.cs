namespace RpcSync.Collect
{
    public interface IIdgeneratorCollect
    {
        int GetWorkId (string mac, int index, long sysTypeId);
    }
}