using WeDonekRpc.Client.Track.Model;
namespace WeDonekRpc.Client.Interface
{
    /// <summary>
    /// 链路序列化
    /// </summary>
    public interface ITackSerialize
    {
        /// <summary>
        /// 序列号
        /// </summary>
        /// <param name="tracks">链路日志</param>
        /// <returns>序列化结果</returns>
        string Serialize(TrackBody[] tracks);
    }
}