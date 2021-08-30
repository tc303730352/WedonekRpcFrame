
using RpcClient.Track;
using RpcClient.Track.Config;
using RpcClient.Track.Model;

using RpcModel.Model;

namespace RpcClient.Interface
{
        public interface ITrackCollect
        {

                /// <summary>
                /// 链路ID
                /// </summary>
                string TraceId { get; }
                /// <summary>
                /// 链路Span
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


                bool CheckIsTrack(TrackRange range, out long traceId);
                /// <summary>
                /// 获取答复模板
                /// </summary>
                /// <returns></returns>
                TrackDepth[] GetAnswerTemplate();
                void EndTrack(TrackBody track);
                TrackSpan CreateTrack(long spanId);
                TrackSpan CreateAnswerTrack(TrackSpan span);
                void SetTrack(TrackSpan track);
                long ApplySpanId();
        }
}