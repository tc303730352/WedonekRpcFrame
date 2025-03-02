namespace AutoTask.Gateway.Interface
{
    public interface IServerConfigService
    {
        string GetName(long id);
        Dictionary<long, string> GetNames(long[] ids);
    }
}