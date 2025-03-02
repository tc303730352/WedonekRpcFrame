using System.IO;

namespace WeDonekRpc.IOSendInterface
{
    /// <summary>
    /// 上传结果
    /// </summary>
    public interface IFileUpResult
    {
        /// <summary>
        /// 耗时
        /// </summary>
        int ConsumeTime { get; }
        /// <summary>
        /// 错误信息
        /// </summary>
        string Error { get; }
        /// <summary>
        /// 上传的文件
        /// </summary>
        FileInfo File { get; }
        /// <summary>
        /// 是否成功
        /// </summary>
        bool IsSuccess { get; }
        /// <summary>
        /// 附带参数(当前传递的值)
        /// </summary>
        object UpParam { get; }

        /// <summary>
        /// 获取返回的结果
        /// </summary>
        /// <returns></returns>
        byte[] GetByte ();
        /// <summary>
        /// 获取返回的结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetObject<T> ();
        /// <summary>
        /// 获取返回的结果
        /// </summary>
        /// <returns></returns>
        string GetString ();
    }
}