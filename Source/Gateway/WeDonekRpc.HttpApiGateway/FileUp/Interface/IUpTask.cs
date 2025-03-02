using WeDonekRpc.Client.Attr;
using WeDonekRpc.HttpApiGateway.FileUp.Model;

namespace WeDonekRpc.HttpApiGateway.FileUp.Interface
{
    [IgnoreIoc]
    public interface IUpTask
    {
        /// <summary>
        /// 上传完成并返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        void UpComplate<T> ( T result );

        /// <summary>
        /// 设置上传的错误状态
        /// </summary>
        /// <param name="error"></param>
        void UpError ( string error );

        /// <summary>
        /// 开始上传
        /// </summary>
        void BeginUp ( UpBasicFile file );
    }

}
