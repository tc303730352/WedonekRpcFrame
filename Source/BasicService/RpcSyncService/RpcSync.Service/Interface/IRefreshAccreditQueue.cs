namespace RpcSync.Service.Interface
{
    public interface IRefreshAccreditQueue
    {
        void Add (string accreditId);
        void Add (string[] accreditId);
    }
}