using Wedonek.Gateway.Modular.Interface;
using Wedonek.Gateway.Modular.Model;
using WeDonekRpc.ApiGateway.Attr;
using WeDonekRpc.HttpApiGateway;
using WeDonekRpc.HttpApiGateway.FileUp.Interface;
using WeDonekRpc.HttpApiGateway.FileUp.Model;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpService.Interface;

namespace Wedonek.Gateway.Modular.FileUp
{
    /// <summary>
    /// 分块上传Demo
    /// </summary>
    [ApiRouteName("/demo/block/up")]
    internal class BlockUpDemo : UpBlockFileTask<UpFileArg>
    {
        private readonly IBlockUpService _Service;

        public BlockUpDemo ( IBlockUpService service )
        {
            this._Service = service;
        }

        /// <summary>
        /// 初始化任务
        /// </summary>
        /// <param name="service">当前请求服务</param>
        /// <param name="task">任务信息</param>
        public override void InitTask ( IApiService service, IBlockUp<UpFileArg> task )
        {
            this._Service.InitTask(task, service.UserState);
        }
        public override void Complete ( IUpFileResult result, IUpFile file )
        {
            this._Service.Complete(result, file);
        }

        public override void UpFail ( UpBasicFile file, string error )
        {
            this._Service.UpFail(file, error);
        }
    }
}
