using System.Collections.Generic;

namespace RpcClient.Track.Config
{
        public class TrackConfig
        {
                /// <summary>
                /// 是否启用
                /// </summary>
                public bool IsEnable
                {
                        get;
                        set;
                }
                /// <summary>
                /// 生成128位的ID
                /// </summary>
                public bool Trace128Bits
                {
                        get;
                        set;
                } = true;
                /// <summary>
                /// 服务名
                /// </summary>
                public string ServiceName
                {
                        get;
                        set;
                }
                /// <summary>
                /// 链路跟踪类型
                /// </summary>
                public TraceType TraceType
                {
                        get;
                        set;
                } = TraceType.Zipkin;
                /// <summary>
                /// 追踪数据的深度
                /// </summary>
                public TrackDepth TrackDepth
                {
                        get;
                        set;
                } = TrackDepth.基本;
                /// <summary>
                /// 链路跟踪范围
                /// </summary>
                public TrackRange TrackRange
                {
                        get;
                        set;
                } = TrackRange.ALL;
                /// <summary>
                /// 抽样率 1/1000000
                /// </summary>
                public int SamplingRate
                {
                        get;
                        set;
                } = 1000000;
                /// <summary>
                /// Zipkin配置
                /// </summary>
                public ZipkinConfig ZipkinTack
                {
                        get;
                        set;
                }
                private TrackDepth[] _csArg = null;
                internal TrackDepth[] GetArgTemplate()
                {
                        if (this._csArg != null)
                        {
                                return this._csArg;
                        }
                        else if (this.TrackDepth == TrackDepth.基本)
                        {
                                this._csArg = new TrackDepth[0];
                                return this._csArg;
                        }
                        List<TrackDepth> list = new List<TrackDepth>(2);
                        if ((TrackDepth.响应的数据 & this.TrackDepth) == TrackDepth.响应的数据)
                        {
                                list.Add(TrackDepth.响应的数据);
                        }
                        if ((TrackDepth.发起的参数 & this.TrackDepth) == TrackDepth.发起的参数)
                        {
                                list.Add(TrackDepth.发起的参数);
                        }
                        this._csArg = list.ToArray();
                        return this._csArg;
                }


                private TrackDepth[] _ssArg = null;

                internal TrackDepth[] GetAnswerTemplate()
                {
                        if (this._ssArg != null)
                        {
                                return this._ssArg;
                        }
                        else if (this.TrackDepth == TrackDepth.基本)
                        {
                                this._ssArg = new TrackDepth[0];
                                return this._ssArg;
                        }
                        List<TrackDepth> list = new List<TrackDepth>(2);
                        if ((TrackDepth.返回的数据 & this.TrackDepth) == TrackDepth.返回的数据)
                        {
                                list.Add(TrackDepth.返回的数据);
                        }
                        if ((TrackDepth.接收的数据 & this.TrackDepth) == TrackDepth.接收的数据)
                        {
                                list.Add(TrackDepth.接收的数据);
                        }
                        this._ssArg = list.ToArray();
                        return this._ssArg;
                }
        }
}
