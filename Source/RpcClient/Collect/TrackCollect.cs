using System;

using RpcClient.Config;
using RpcClient.Helper;
using RpcClient.Interface;
using RpcClient.Track;
using RpcClient.Track.Config;
using RpcClient.Track.Model;

using RpcHelper;

using RpcModel.Model;

namespace RpcClient.Collect
{
        [Attr.ClassLifetimeAttr(Attr.ClassLifetimeType.单例)]
        internal class TrackCollect : ITrackCollect
        {
                [ThreadStatic]
                private static TrackSpan _Span = null;
                [ThreadStatic]
                private static TrackSpan _CurrentSpan = null;

                public string TraceId => _Span?.ToTraceId();

                private static TrackConfig _Config = new TrackConfig();
                private static ISampler _Sampler = null;
                public static bool IsEnable => _Config.IsEnable;

                public bool CheckIsTrack(TrackRange range, out long spanId)
                {
                        if (_Span != null)
                        {
                                spanId = _ApplySpanId();
                                return true;
                        }
                        else if (!_Config.IsEnable || (range & _Config.TrackRange) != range)
                        {
                                spanId = 0;
                                return false;
                        }
                        return _Sampler.Sample(out spanId);
                }

                private static ITack _Tack = null;

                public static event Action<TrackConfig> EnableChange;

                static TrackCollect()
                {
                        RpcClient.Config.AddRefreshEvent((a,name) => {
                                if (name.StartsWith("track") || name == string.Empty)
                                {
                                        Init();
                                }
                        });
                }
                public static void Init()
                {
                        if (!RpcStateCollect.IsInit)
                        {
                                return;
                        }
                        TrackConfig config = WebConfig.GetTrackConfig();
                        if (config.IsEnable && (config.ZipkinTack == null || config.ZipkinTack.PostUri == null))
                        {
                                config.IsEnable = false;
                        }
                        if (_Sampler == null || config.SamplingRate != _Config.SamplingRate)
                        {
                                _Sampler = new DefaultSampler(_ApplySpanId(), config.SamplingRate);
                        }
                        if (config.IsEnable && (_Tack == null || config.ZipkinTack != _Config.ZipkinTack || config.ServiceName != _Config.ServiceName))
                        {
                                _Tack = new ZipkinTack(config.ZipkinTack, config.ServiceName);
                        }
                        _Config = config;
                        if (config.IsEnable != _Config.IsEnable && EnableChange != null)
                        {
                                EnableChange(config);
                        }
                }

                internal static void ClearTrack(TrackSpan span)
                {
                        if (span.ParentId.HasValue && span.SpanId != _Span.SpanId)
                        {
                                _CurrentSpan = null;
                        }
                        else
                        {
                                _Span = null;
                                _CurrentSpan = null;
                        }
                }

                public TrackConfig Config => _Config;

                public static TrackSpan CurrentSpan => _CurrentSpan;

                public TrackSpan TrackSpan => _CurrentSpan;


                private TrackSpan _CreateTrack(long spanId)
                {
                        return new TrackSpan
                        {
                                TraceId = _ApplySpanId(),
                                HighTraceId = _Config.Trace128Bits ? _ApplySpanId() : 0,
                                SpanId = spanId
                        };
                }
                private TrackSpan _CreateTrack(TrackSpan span, long spanId)
                {
                        return new TrackSpan
                        {
                                TraceId = span.TraceId,
                                HighTraceId = span.HighTraceId,
                                SpanId = spanId,
                                ParentId = span.SpanId
                        };
                }
                public TrackSpan CreateTrack(long spanId)
                {
                        if (_Span == null)
                        {
                                _Span = this._CreateTrack(spanId);
                                _CurrentSpan = _Span;
                                return _CurrentSpan;
                        }
                        _CurrentSpan = this._CreateTrack(_Span, spanId);
                        return _CurrentSpan;
                }

                public void EndTrack(TrackBody track)
                {
                        DateTime now = DateTime.Now;
                        track.Annotations[1].Time = now;
                        track.Duration = (long)((now - track.Time).TotalMilliseconds * 1000L);
                        _Tack.AddTrace(track);
                }

                public TrackSpan CreateAnswerTrack(TrackSpan span)
                {
                        if (span == null)
                        {
                                _Span = this._CreateTrack(_ApplySpanId());
                                _CurrentSpan = _Span;
                                return _Span;
                        }
                        _Span = this._CreateTrack(span, _ApplySpanId());
                        _CurrentSpan = _Span;
                        return _CurrentSpan;
                }
                /// <summary>
                /// 获取发起的模板
                /// </summary>
                /// <returns></returns>
                public TrackDepth[] GetArgTemplate()
                {
                        return _Config.GetArgTemplate();
                }
                public TrackDepth[] GetAnswerTemplate()
                {
                        return _Config.GetAnswerTemplate();
                }

                public void SetTrack(TrackSpan track)
                {
                        _Span = track;
                        _CurrentSpan = track;
                }

                public long ApplySpanId()
                {
                        return _ApplySpanId();
                }
                private static long _ApplySpanId()
                {
                        return RandomUtils.NextLong();
                }
        }
}
