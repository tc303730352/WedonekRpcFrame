namespace WeDonekRpc.HttpApiGateway.Interface
{
    public interface IHttpRouteService
    {
        void EnableByPath(string path);

        void DisableByPath(string path);

        void Enable(string routeId);

        void Disable(string routeId);

        void RemoveByPath(string path);

        void Remove(string routeId);
    }
}