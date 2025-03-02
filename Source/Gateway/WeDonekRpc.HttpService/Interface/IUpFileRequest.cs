using System;
using System.IO;
using WeDonekRpc.HttpService.FileUp;
namespace WeDonekRpc.HttpService.Interface
{
    public interface IUpFileRequest
    {
        /// <summary>
        /// 是否计算MD5
        /// </summary>
        bool IsGenerateMd5 { get; }
        /// <summary>
        /// 检查上传文件参数
        /// </summary>
        /// <param name="param"></param>
        void CheckUpFile(UpFileParam param);
        /// <summary>
        /// 获取保存的数据流
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Stream GetSaveStream(UpFileParam param);

        /// <summary>
        /// 文件上传请求
        /// </summary>
        void InitFileUp();
        /// <summary>
        /// 保存上传流
        /// </summary>
        /// <param name="upParam"></param>
        /// <param name="stream"></param>
        void SaveUpStream(UpFileParam upParam, Stream stream);

        /// <summary>
        /// 上传失败
        /// </summary>
        /// <param name="e"></param>
        void UpFail(Exception e);
        /// <summary>
        /// 验证文件
        /// </summary>
        /// <param name="upParam"></param>
        /// <param name="length"></param>
        void VerificationFile(UpFileParam upParam, long length);
    }
}
