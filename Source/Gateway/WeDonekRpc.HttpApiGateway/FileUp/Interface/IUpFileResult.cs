using WeDonekRpc.Client.Attr;
using WeDonekRpc.HttpApiGateway.FileUp.Model;

namespace WeDonekRpc.HttpApiGateway.FileUp.Interface
{
    [IgnoreIoc]
    public interface IUpFileResult
    {
        /// <summary>
        /// 文件信息
        /// </summary>
        UpBasicFile File {  get; }

        /// <summary>
        /// 上传完成并返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        void UpComplate<T> ( T result );


    }
}
