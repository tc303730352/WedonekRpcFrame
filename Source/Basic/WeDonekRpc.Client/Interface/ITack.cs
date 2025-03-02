using WeDonekRpc.Client.Track.Model;

namespace WeDonekRpc.Client.Interface
{
    /// <summary>
    /// 链接跟踪记录器
    /// </summary>
    public interface ITack : System.IDisposable
    {
        /// <summary>
        /// 添加链路日志
        /// </summary>
        /// <param name="track">链路信息</param>
        void AddTrace(TrackBody track);
    }
}