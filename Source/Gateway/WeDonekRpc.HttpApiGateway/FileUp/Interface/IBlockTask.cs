using System.IO;
using WeDonekRpc.Client.Attr;

namespace WeDonekRpc.HttpApiGateway.FileUp.Interface
{
    [IgnoreIoc]
    public interface IBlockTask
    {
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="path"></param>
        FileInfo Save ( string path );

        void UpComplate<T> ( T result );

        /// <summary>
        /// 上传错误
        /// </summary>
        /// <param name="error"></param>
        void UpError ( string error );
    }
}
