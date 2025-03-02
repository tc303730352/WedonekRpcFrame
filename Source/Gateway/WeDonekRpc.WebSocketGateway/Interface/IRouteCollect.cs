namespace WeDonekRpc.WebSocketGateway.Interface
{
    public interface IRouteCollect
    {
        void EnableByPath (string path);

        void DisableByPath (string path);

        void RemoveByPath (string path);
    }
}