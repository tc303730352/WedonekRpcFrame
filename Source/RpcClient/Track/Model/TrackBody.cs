using System;

using RpcModel.Model;

namespace RpcClient.Track.Model
{

        public class TrackBody : System.IDisposable
        {
                /// <summary>
                /// 高位
                /// </summary>
                public TrackSpan Trace;

                /// <summary>
                /// 方法
                /// </summary>
                public string Dictate;
                /// <summary>
                /// 系统类型
                /// </summary>
                public string RemoteIp;
                /// <summary>
                /// 端口
                /// </summary>
                public int Port;
                /// <summary>
                /// 开始时间
                /// </summary>
                public DateTime Time;
                public long Duration;
                /// <summary>
                /// 阶段
                /// </summary>
                public TrackAnnotation[] Annotations;

                public TrackArg[] Args;

                public string ServerName { get; set; }

                public void Dispose()
                {
                        Collect.TrackCollect.ClearTrack(this.Trace);
                }
        }
}
