namespace WeDonekRpc.Client.Interface
{
    public interface ILocalEventCollect
    {
        void Public (object eventData, string name = null);
        void AsyncPublic (object eventData, string name = null);
    }
}