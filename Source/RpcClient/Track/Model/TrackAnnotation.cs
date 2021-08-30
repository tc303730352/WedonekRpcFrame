using System;

namespace RpcClient.Track.Model
{
        public class TrackAnnotation
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
