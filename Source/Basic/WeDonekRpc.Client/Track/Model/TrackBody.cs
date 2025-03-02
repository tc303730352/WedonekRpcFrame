using System;
using WeDonekRpc.Client.Collect;
using WeDonekRpc.ExtendModel;
using WeDonekRpc.Model.Model;

namespace WeDonekRpc.Client.Track.Model
{
    /// <summary>
    /// 链路结构
    /// </summary>
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
        /// 方法类型
        /// </summary>
        public StageType StageType;
        /// <summary>
        /// 远程IP
        /// </summary>
        public string RemoteIp;
        /// <summary>
        /// 远程ID
        /// </summary>
        public long? RemoteId;
        /// <summary>
        /// 端口
        /// </summary>
        public int Port;
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime Time;
        /// <summary>
        /// 执行时间
        /// </summary>
        public int Duration;
        /// <summary>
        /// 阶段
        /// </summary>
        public TrackAnnotation[] Annotations;
        /// <summary>
        /// 参数
        /// </summary>
        public TrackArg[] Args;
        /// <summary>
        /// 说明
        /// </summary>
        public string Show { get; set; }
        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="isSuccess">是否成功</param>
        public void Commit (bool isSuccess)
        {
            QueueTrackCollect.EndTrack(this, isSuccess);
        }

        public void Dispose ()
        {
            Collect.TrackCollect.ClearTrack(this.Trace);
        }
    }
}
