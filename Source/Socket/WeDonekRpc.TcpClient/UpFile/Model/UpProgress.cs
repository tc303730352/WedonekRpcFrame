using WeDonekRpc.IOSendInterface;

namespace WeDonekRpc.TcpClient.UpFile.Model
{
    /// <summary>
    /// 上传进度
    /// </summary>
    internal class UpProgress : IUpProgress
    {
        public UpProgress (UpFileTask task)
        {
            this.UpSpeed = task.GetUpSpeed(out long upNum, out int time);
            this.Progress = task.UpProgress;
            this.AlreadyUpNum = upNum;
            this.ConsumeTime = time;
        }
        /// <summary>
        /// 上传进度
        /// </summary>
        public UpFileProgress Progress
        {
            get;
        }

        /// <summary>
        /// 已上传数据量
        /// </summary>
        public long AlreadyUpNum
        {
            get;
        }
        /// <summary>
        /// 消耗时间
        /// </summary>
        public int ConsumeTime { get; }

        /// <summary>
        /// 上传速度
        /// </summary>
        public long UpSpeed
        {
            get;
        }

    }
}
