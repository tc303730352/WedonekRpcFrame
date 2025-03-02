namespace RpcCentral.DAL
{
    public interface IServiceVerConfigDAL
    {
        long GetVerId (int ver, long systemTypeId, long rpcMerId);
    }
}