using WeDonekRpc.HttpApiGateway.FileUp.Interface;
using WeDonekRpc.HttpApiGateway.FileUp.Model;
using WeDonekRpc.HttpService.Interface;

namespace WeDonekRpc.HttpApiGateway.Interface
{
    public interface IUpBlockFileTask : IApiGateway
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="service">服务信息</param>
        /// <param name="task">任务</param>
        void Init ( IApiService service, IUpTask task );

        /// <summary>
        /// 上传失败
        /// </summary>
        /// <param name="data">文件信息</param>
        /// <param name="error">错误信息</param>
        void UpFail ( UpBasicFile data, string error );

        /// <summary>
        /// 上传完成
        /// </summary>
        /// <param name="result">结果</param>
        /// <param name="upFile">上传文件</param>
        void Complete ( IUpFileResult result, IUpFile upFile);
    }
}
