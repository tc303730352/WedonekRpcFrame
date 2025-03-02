namespace RpcCentral.DAL
{
    public interface IServiceVerRouteDAL
    {
        Dictionary<long, int> GetVerRoute (long verId);
    }
}