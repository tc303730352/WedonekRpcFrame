using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.HttpApiGateway.Route;
using WeDonekRpc.HttpService.Config;

namespace WeDonekRpc.HttpApiGateway.Interface
{
    public interface IHttpGatewayOption
    {
        /// <summary>
        /// 配置项
        /// </summary>
        IGatewayOption Option { get; }
        /// <summary>
        /// 添加文件目录
        /// </summary>
        /// <param name="dir"></param>
        void AddFileDir (FileDirConfig dir);
        /// <summary>
        /// IOC容器
        /// </summary>
        IocBuffer IocBuffer { get; }

        /// <summary>
        /// 路由
        /// </summary>
        RouteBuffer Route { get; }

        void SetDefApiServiceEvent<T> () where T : class, IApiServiceEvent;
        /// <summary>
        /// 注册模块
        /// </summary>
        /// <param name="modular"></param>
        void RegModular (IModular modular);
    }
}
