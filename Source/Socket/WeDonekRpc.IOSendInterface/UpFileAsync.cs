using System.IO;

namespace WeDonekRpc.IOSendInterface
{
    /// <summary>
    /// 上传结果回调
    /// </summary>
    /// <param name="result"></param>
    public delegate void UpFileAsync (IFileUpResult result);

    /// <summary>
    /// 上传进度
    /// </summary>
    /// <param name="file">上传的文件</param>
    /// <param name="progress">进度0-100</param>
    /// <param name="alreadyUpNum">已经上传了的字节数</param>
    public delegate void UpProgressAction (FileInfo file, int progress, long alreadyUpNum);
}
