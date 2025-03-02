namespace RpcSync.Service.Interface
{
    public interface IClearAccreditQueue
    {
        void Add (string accreditId);
        void Add (string[] accreditId);
    }
}