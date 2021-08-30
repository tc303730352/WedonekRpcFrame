using System.IO;

namespace HttpService
{
        public interface IUpFile : System.IDisposable
        {
                /// <summary>
                /// 内容类型
                /// </summary>
                string ContentType { get; }
                /// <summary>
                /// 文件名
                /// </summary>
                string FileName { get; }
                /// <summary>
                /// 文件大小
                /// </summary>
                long FileSize { get; }
                /// <summary>
                /// 文件类型
                /// </summary>
                string FileType { get; }

                string FileMd5 { get; }
                /// <summary>
                /// 读取流
                /// </summary>
                /// <returns></returns>
                byte[] ReadStream();

                /// <summary>
                /// 获取流对象
                /// </summary>
                /// <returns></returns>
                Stream GetStream();

                /// <summary>
                /// 保存文件
                /// </summary>
                /// <param name="savePath"></param>
                string SaveFile(string savePath);
        }
}