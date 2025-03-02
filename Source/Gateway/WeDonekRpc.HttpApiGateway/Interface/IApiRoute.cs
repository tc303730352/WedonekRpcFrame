using WeDonekRpc.ApiGateway.Model;
using WeDonekRpc.Client.Ioc;
using System.Reflection;

namespace WeDonekRpc.HttpApiGateway.Interface
{
    internal interface IApiRoute : IRoute, System.IDisposable
    {
        bool IsRegex { get; }

        bool IsEnable { get; }

        void Enable ();

        void Disable ();

        string ApiEventType { get; }
        /// <summary>
        /// 获取上传的配置信息
        /// </summary>
        /// <returns></returns>
        ApiUpSet GetUpSet ();
        /// <summary>
        /// 获取上传配置
        /// </summary>
        /// <returns></returns>
        IUpFileConfig CreateUpFileConfig ( IocScope scope );

        MethodInfo Source { get; }
        /// <summary>
        /// 执行API
        /// </summary>
        /// <param name="service"></param>
        void ExecApi ( IService service );
        /// <summary>
        /// 响应请求
        /// </summary>
        /// <param name="service"></param>
        void ReceiveRequest ( IService service );
        /// <summary>
        /// 初始化路由
        /// </summary>
        void InitRoute ();
    }
}