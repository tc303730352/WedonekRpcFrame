using WeDonekRpc.HttpService.Interface;

namespace WeDonekRpc.HttpApiGateway.Interface
{
    public interface IHttpPlugIn : System.IDisposable
        {
                /// <summary>
                /// 插件名称(唯一)
                /// </summary>
                string Name { get; }
                /// <summary>
                /// 是否启用
                /// </summary>
                bool IsEnable { get; }
                /// <summary>
                /// 初始化插件
                /// </summary>
                /// <returns>是否启用</returns>
                void Init();
                /// <summary>
                /// 执行插件
                /// </summary>
                /// <param name="route"></param>
                /// <param name="handler"></param>
                void Exec(IRoute route, IHttpHandler handler);
        }
}