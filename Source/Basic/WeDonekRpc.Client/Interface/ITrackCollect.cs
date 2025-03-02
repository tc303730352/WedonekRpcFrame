
using WeDonekRpc.Client.Track;
using WeDonekRpc.Client.Track.Config;
using WeDonekRpc.Client.Track.Model;

using WeDonekRpc.Model.Model;

namespace WeDonekRpc.Client.Interface
{
    /// <summary>
    /// 链路跟踪集合
    /// </summary>
    public interface ITrackCollect
    {

        /// <summary>
        /// 链路ID
        /// </summary>
        string TraceId { get; }
        /// <summary>
        /// 链路跨度
        /// </summary>
        TrackSpan TrackSpan { get; }
        /// <summary>
        /// 链路配置
        /// </summary>
        TrackConfig Config { get; }
        /// <summary>
        /// 获取发起请求的深度
        /// </summary>
        /// <returns></returns>
        TrackDepth[] GetArgTemplate();

        /// <summary>
        /// 检查是否采样
        /// </summary>
        /// <param name="range">采样类型</param>
        /// <param name="spanId">当前链路跨度ID</param>
        /// <returns>是否采样</returns>
        bool CheckIsTrack(TrackRange range, out long spanId);
        /// <summary>
        /// 获取答复模板
        /// </summary>
        /// <returns></returns>
        TrackDepth[] GetAnswerTemplate();
        /// <summary>
        /// 结束采样
        /// </summary>
        /// <param name="track">采样的链路结构</param>
        void EndTrack(TrackBody track);
        /// <summary>
        /// 创建采样的链路跨度
        /// </summary>
        /// <param name="spanId">当前跨度ID</param>
        /// <returns>链路的跨度信息</returns>
        TrackSpan CreateTrack(long spanId);
        /// <summary>
        /// 创建应答的链路跨度信息
        /// </summary>
        /// <param name="span">当前链路的跨度信息</param>
        /// <returns>应答的链路的跨度信息</returns>
        TrackSpan CreateAnswerTrack(TrackSpan span);
        /// <summary>
        /// 设置当前线程所属的链路跨度
        /// </summary>
        /// <param name="span">当前链路的跨度信息</param>
        void SetTrack(TrackSpan span);
        /// <summary>
        /// 申请一个链路跨度ID
        /// </summary>
        /// <returns></returns>
        long ApplySpanId();
    }
}