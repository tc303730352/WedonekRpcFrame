using System.IO;

namespace WeDonekRpc.Client.FileUp
{
    public interface IUpResult
    {
        /// <summary>
        /// 文件MD5
        /// </summary>
        string FileMd5 { get; }

        /// <summary>
        /// 获取所有字节
        /// </summary>
        /// <returns></returns>
        byte[] GetByte ();

        /// <summary>
        /// 获取上传的文件流
        /// </summary>
        /// <returns></returns>
        Stream GetStream ();


        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="savePath"></param>
        void Save ( string savePath );

    }
}