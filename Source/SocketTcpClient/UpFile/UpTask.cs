using System.IO;

namespace SocketTcpClient.UpFile
{
        /// <summary>
        /// 上传任务
        /// </summary>
        public class UpTask
        {
                public UpTask(string taskId, string serverId)
                {
                        this.TaskId = taskId;
                        this.ServerId = serverId;
                }
                /// <summary>
                /// 文件信息
                /// </summary>
                public FileInfo File
                {
                        get;
                }
                /// <summary>
                /// 上传参数
                /// </summary>
                public object UpParam
                {
                        get;
                }
                /// <summary>
                /// 服务器ID
                /// </summary>
                public string ServerId
                {
                        get;
                }
                /// <summary>
                /// 任务ID
                /// </summary>
                public string TaskId
                {
                        get;
                }
                /// <summary>
                /// 取消上传
                /// </summary>
                public void CancelUp()
                {
                        UpFileTaskCollect.CacnelTask(this.TaskId);
                }
        }
}
