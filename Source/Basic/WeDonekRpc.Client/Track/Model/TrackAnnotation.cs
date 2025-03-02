using System;

namespace WeDonekRpc.Client.Track.Model
{
    /// <summary>
    /// 链路注解
    /// </summary>
    public struct TrackAnnotation
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime Time;
        /// <summary>
        /// 阶段
        /// </summary>
        public TrackStage Stage
        {
            get;
            set;
        }
    }
}
