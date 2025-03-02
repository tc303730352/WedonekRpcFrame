using System;
using System.Threading;
using WeDonekRpc.Client.Helper;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Track;
using WeDonekRpc.Client.Track.Config;
using WeDonekRpc.Client.Track.Model;
using WeDonekRpc.Model.Model;

namespace WeDonekRpc.Client.Collect
{
    [Attr.ClassLifetimeAttr(Attr.ClassLifetimeType.SingleInstance)]
    internal class TrackCollect : ITrackCollect
    {
        /// <summary>
        /// 跨度
        /// </summary>
        private static readonly AsyncLocal<TrackSpan> _Span = new AsyncLocal<TrackSpan>();
        /// <summary>
        /// 当前跨度
        /// </summary>
        private static readonly AsyncLocal<TrackSpan> _CurrentSpan = new AsyncLocal<TrackSpan>();
        /// <summary>
        /// 跨度ID
        /// </summary>
        public string TraceId => _Span.Value?.ToTraceId();
        /// <summary>
        /// 链路配置
        /// </summary>
        private static readonly TrackConfig _Config;
        /// <summary>
        /// 采样器
        /// </summary>
        private static ISampler _Sampler = null;

        private static ITack _Tack = null;

        /// <summary>
        /// 是否启用
        /// </summary>
        public static bool IsEnable => _Config.IsEnable;

        public TrackConfig Config => _Config;

        public static TrackSpan CurrentSpan => _CurrentSpan.Value;

        public TrackSpan TrackSpan => _CurrentSpan.Value;

        public static event Action<TrackConfig> EnableChange;
        /// <summary>
        /// 检查是否采样
        /// </summary>
        /// <param name="range"></param>
        /// <param name="spanId"></param>
        /// <returns></returns>
        public bool CheckIsTrack (TrackRange range, out long spanId)
        {
            if (_Span.Value != null)
            {
                spanId = _ApplySpanId();
                return true;
            }
            else if (_Tack == null || !_Config.IsEnable || ( range & _Config.TrackRange ) != range)
            {
                spanId = 0;
                return false;
            }
            return _Sampler.Sample(out spanId);
        }



        static TrackCollect ()
        {
            _Config = new TrackConfig(_Refresh);
        }
        private static void _Refresh (TrackConfig config, string name)
        {
            if (_Sampler == null || name == "SamplingRate")
            {
                _Sampler = new DefaultSampler(_ApplySpanId(), config.SamplingRate);
            }
            if (name == "IsEnable")
            {
                if (config.IsEnable)
                {
                    _Tack?.Dispose();
                    _InitTrack(config);
                }
                if (EnableChange != null)
                {
                    EnableChange(config);
                }
            }
        }

        /// <summary>
        /// 初始化链路追踪
        /// </summary>
        public static void Init ()
        {
            if (!RpcStateCollect.IsInit)
            {
                return;
            }
            if (_Sampler == null)
            {
                _Sampler = new DefaultSampler(_ApplySpanId(), _Config.SamplingRate);
            }
            if (_Config.IsEnable && _Tack == null)
            {
                _InitTrack(_Config);
            }
            if (EnableChange != null)
            {
                EnableChange(_Config);
            }
        }
        private static void _InitTrack (TrackConfig config)
        {
            if (config.TraceType == TraceType.Zipkin)
            {
                _Tack = new ZipkinTack(config.ZipkinTack);
            }
            else
            {
                _Tack = new LocalTack(config.Local);
            }
        }
        internal static void ClearTrack (TrackSpan span)
        {
            if (span.ParentId.HasValue && span.SpanId != _Span.Value?.SpanId)
            {
                _CurrentSpan.Value = null;
            }
            else
            {
                _Span.Value = null;
                _CurrentSpan.Value = null;
            }
        }
        private TrackSpan _CreateTrack (long spanId)
        {
            return new TrackSpan
            {
                TraceId = _ApplySpanId(),
                HighTraceId = _Config.Trace128Bits ? _ApplySpanId() : 0,
                SpanId = spanId
            };
        }
        private TrackSpan _CreateTrack (TrackSpan span, long spanId)
        {
            return new TrackSpan
            {
                TraceId = span.TraceId,
                HighTraceId = span.HighTraceId,
                SpanId = spanId,
                ParentId = span.SpanId
            };
        }
        public TrackSpan CreateTrack (long spanId)
        {
            if (_Span.Value == null)
            {
                TrackSpan span = this._CreateTrack(spanId);
                _Span.Value = span;
                _CurrentSpan.Value = span;
                return span;
            }
            _CurrentSpan.Value = this._CreateTrack(_Span.Value, spanId);
            return _CurrentSpan.Value;
        }

        public void EndTrack (TrackBody track)
        {
            DateTime now = DateTime.Now;
            track.Annotations[1].Time = now;
            track.Duration = (int)( ( now - track.Time ).TotalMilliseconds * 1000L );
            _Tack.AddTrace(track);
        }

        public TrackSpan CreateAnswerTrack (TrackSpan span)
        {
            TrackSpan track = span == null ? this._CreateTrack(_ApplySpanId()) : this._CreateTrack(span, _ApplySpanId());
            this.SetTrack(track);
            return track;
        }
        /// <summary>
        /// 获取发起的模板
        /// </summary>
        /// <returns></returns>
        public TrackDepth[] GetArgTemplate ()
        {
            return _Config.GetArgTemplate();
        }
        public TrackDepth[] GetAnswerTemplate ()
        {
            return _Config.GetAnswerTemplate();
        }

        public void SetTrack (TrackSpan track)
        {
            _Span.Value = track;
            _CurrentSpan.Value = track;
        }

        public long ApplySpanId ()
        {
            return _ApplySpanId();
        }
        private static long _ApplySpanId ()
        {
            return RandomUtils.NextLong();
        }
    }
}
