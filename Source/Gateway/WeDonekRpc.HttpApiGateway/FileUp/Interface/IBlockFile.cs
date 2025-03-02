using System.IO;

namespace WeDonekRpc.HttpApiGateway.FileUp.Interface
{
    public interface IBlockFile
    {
        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>已保存的文件</returns>
        string Save ( string path, bool overwrite = true );


        /// <summary>
        /// 获取数据流
        /// </summary>
        /// <returns></returns>
        Stream GetStream ();

        /// <summary>
        /// 复制流
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        long CopyStream ( Stream stream, int offset );
        /// <summary>
        /// 获取留
        /// </summary>
        /// <returns></returns>
        byte[] GetBytes ();
    }
}