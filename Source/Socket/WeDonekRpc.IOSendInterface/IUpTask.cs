using System.IO;

namespace WeDonekRpc.IOSendInterface
{
    /// <summary>
    /// 上传任务
    /// </summary>
    public interface IUpTask
    {
        /// <summary>
        /// 上传的文件
        /// </summary>
        FileInfo File { get; }
        /// <summary>
        /// 上传的目的地
        /// </summary>
        string ServerId { get; }
        /// <summary>
        /// 任务ID
        /// </summary>
        string TaskId { get; }
        /// <summary>
        /// 上传附带参数
        /// </summary>
        object UpParam { get; }

        /// <summary>
        /// 取消上传
        /// </summary>
        void CancelUp ();
    }
}