using System.IO;

namespace WeDonekRpc.HttpService.Interface
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
        /// <summary>
        /// 文件MD5
        /// </summary>
        string FileMd5 { get; }
        /// <summary>
        /// 读取流
        /// </summary>
        /// <returns></returns>
        byte[] ReadStream ();

        /// <summary>
        /// 获取流对象
        /// </summary>
        /// <returns></returns>
        Stream GetStream ();
        /// <summary>
        /// 将文件流写入另外一个流中
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="offset"></param>
        long CopyStream ( Stream stream, int offset );
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="savePath"></param>
        string SaveFile ( string savePath, bool overwrite = false );
    }
}