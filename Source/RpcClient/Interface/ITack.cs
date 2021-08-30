using RpcClient.Track.Model;

namespace RpcClient.Interface
{
        public interface ITack : System.IDisposable
        {
                void AddTrace(TrackBody track);
        }
}