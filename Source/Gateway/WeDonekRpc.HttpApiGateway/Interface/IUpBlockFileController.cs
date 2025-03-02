using WeDonekRpc.Client.Attr;
using WeDonekRpc.HttpApiGateway.FileUp.Interface;
using WeDonekRpc.HttpApiGateway.FileUp.Model;
using WeDonekRpc.HttpService.Interface;
using WeDonekRpc.Modular;

namespace WeDonekRpc.HttpApiGateway.Interface
{
    [IgnoreIoc]
    public interface IUpBlockFileController<T>
    {
        /// <summary>
        /// 文件
        /// </summary>
        UpFileData<T> File { get; }

        /// <summary>
        /// 服务名
        /// </summary>
        string ServiceName { get; }

        /// <summary>
        /// 用户状态
        /// </summary>
        IUserState UserState { get; }

        /// <summary>
        /// 请求
        /// </summary>
        IHttpRequest Request
        {
            get;
        }

        /// <summary>
        /// 准备上传任务
        /// </summary>
        void InitUp ( IUpTask task );

    }
    [IgnoreIoc]
    public interface IUpBlockFileController
    {
        /// <summary>
        /// 文件
        /// </summary>
        UpFileData File { get; }
        /// <summary>
        /// 服务名
        /// </summary>
        string ServiceName { get; }

        /// <summary>
        /// 用户状态
        /// </summary>
        IUserState UserState { get; }

        /// <summary>
        /// 请求
        /// </summary>
        IHttpRequest Request
        {
            get;
        }

        /// <summary>
        /// 准备上传任务
        /// </summary>
        void InitUp ( IBlockUp task );
    }
}
