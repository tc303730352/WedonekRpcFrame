using RpcClient.Track.Model;
namespace RpcClient.Interface
{
        public interface ITackSerialize
        {
                byte[] Serialize(TrackBody[] tracks);
        }
}