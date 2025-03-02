using System;
using System.Collections.Generic;
using WeDonekRpc.Helper.Config;

namespace WeDonekRpc.Client.Track.Config
{
    /// <summary>
    /// 链路配置
    /// </summary>
    public class TrackConfig
    {
        private readonly Action<TrackConfig, string> _Refresh;
        internal TrackConfig (Action<TrackConfig, string> refresh)
        {
            this._Refresh = refresh;
            RpcClient.Config.GetSection("rpcassembly:Track").AddRefreshEvent(this._InitConfig);
        }

        private void _InitConfig (IConfigSection section, string name)
        {
            this.IsEnable = section.GetValue("IsEnable", true);
            this.Trace128Bits = section.GetValue("Trace128Bits", true);
            this.ServiceName = section.GetValue("ServiceName");
            this.TraceType = section.GetValue("TraceType", TraceType.Local);
            this.TrackDepth = section.GetValue("TrackDepth", TrackDepth.基本);
            this.TrackRange = section.GetValue("TrackRange", TrackRange.ALL);
            this.SamplingRate = section.GetValue("SamplingRate", 10000);
            this.ZipkinTack = new ZipkinConfig
            {
                PostUri = section.GetValue<Uri>("ZipkinTack:PostUri")
            };
            this.Local = new LocalTrackConfig
            {
                Dictate = section.GetValue("Local:Dictate", "SysTrace"),
                SystemType = section.GetValue("Local:SystemType", "sys.extend"),
            };
            if (this.IsEnable && this.IsNull())
            {
                this.IsEnable = false;
            }
            if (this._Refresh != null)
            {
                this._Refresh(this, name);
            }
        }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable
        {
            get;
            private set;
        }
        /// <summary>
        /// 生成128位的ID
        /// </summary>
        public bool Trace128Bits
        {
            get;
            private set;
        }
        /// <summary>
        /// 服务名
        /// </summary>
        public string ServiceName
        {
            get;
            private set;
        }
        /// <summary>
        /// 链路跟踪类型
        /// </summary>
        public TraceType TraceType
        {
            get;
            private set;
        }
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
        } = 10000;
        /// <summary>
        /// Zipkin配置
        /// </summary>
        public ZipkinConfig ZipkinTack
        {
            get;
            set;
        }
        public LocalTrackConfig Local
        {
            get;
            set;
        }
        private TrackDepth[] _csArg = null;

        public bool IsNull ()
        {
            if (this.TraceType == TraceType.Zipkin)
            {
                return this.ZipkinTack == null || this.ZipkinTack.PostUri == null;
            }
            return false;
        }
        internal TrackDepth[] GetArgTemplate ()
        {
            if (this._csArg != null)
            {
                return this._csArg;
            }
            else if (this.TrackDepth == TrackDepth.基本)
            {
                this._csArg = Array.Empty<TrackDepth>();
                return this._csArg;
            }
            List<TrackDepth> list = new List<TrackDepth>(2);
            if (( TrackDepth.响应的数据 & this.TrackDepth ) == TrackDepth.响应的数据)
            {
                list.Add(TrackDepth.响应的数据);
            }
            if (( TrackDepth.发起的参数 & this.TrackDepth ) == TrackDepth.发起的参数)
            {
                list.Add(TrackDepth.发起的参数);
            }
            this._csArg = list.ToArray();
            return this._csArg;
        }


        private TrackDepth[] _ssArg = null;

        internal TrackDepth[] GetAnswerTemplate ()
        {
            if (this._ssArg != null)
            {
                return this._ssArg;
            }
            else if (this.TrackDepth == TrackDepth.基本)
            {
                this._ssArg = Array.Empty<TrackDepth>();
                return this._ssArg;
            }
            List<TrackDepth> list = new List<TrackDepth>(2);
            if (( TrackDepth.返回的数据 & this.TrackDepth ) == TrackDepth.返回的数据)
            {
                list.Add(TrackDepth.返回的数据);
            }
            if (( TrackDepth.接收的数据 & this.TrackDepth ) == TrackDepth.接收的数据)
            {
                list.Add(TrackDepth.接收的数据);
            }
            this._ssArg = list.ToArray();
            return this._ssArg;
        }
        public bool IsEquals (TrackConfig config)
        {
            if (this.TraceType != config.TraceType)
            {
                return false;
            }
            else if (this.TraceType == TraceType.Zipkin)
            {
                return config.ZipkinTack != this.ZipkinTack || config.ServiceName != this.ServiceName;
            }
            else
            {
                return config.Local != this.Local || config.ServiceName != this.ServiceName;
            }
        }
    }
}
