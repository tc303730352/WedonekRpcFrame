using WeDonekRpc.TcpServer.FileUp.Model;
using System;

namespace WeDonekRpc.TcpServer.Interface
{
    public interface IStreamAllot : ICloneable, IDisposable
    {
        string FileId { get; }

        /// <summary>
        /// 上传配置
        /// </summary>
        FileUpConfig UpConfig
        {
            get;
        }
        /// <summary>
        /// 指令名
        /// </summary>
        string DirectName { get; }
        /// <summary>
        /// 文件授权检查
        /// </summary>
        /// <param name="file">文件信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>是否授权</returns>
        bool FileAccredit(UpFile file, out string error);
        /// <summary>
        /// 检查文件是否存在
        /// </summary>
        /// <returns></returns>
        bool CheckIsExists();

        /// <summary>
        /// 初始化文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        bool InitFile(UpFile file, out string error);

        /// <summary>
        /// 写入流
        /// </summary>
        /// <param name="blockId">块ID</param>
        /// <param name="stream"></param>
        /// <param name="begin"></param>
        /// <param name="count"></param>
        bool Write(ushort blockId, byte[] stream, int begin, int count);

        /// <summary>
        /// 获取文件上传状态
        /// </summary>
        /// <returns></returns>
        FileUpState GetFileUpState();
        /// <summary>
        /// 保存文件流
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        bool SaveFileStream(out string error);

        /// <summary>
        /// 上传超时
        /// </summary>
        void UpTimeOut();

        /// <summary>
        /// 上传完成
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        bool UpComplate(UpFile file, out byte[] result, out string error);
        /// <summary>
        /// 上传错误
        /// </summary>
        /// <param name="file"></param>
        void UpError(UpFile file, string error);
        void DeleteFile();
    }
}
