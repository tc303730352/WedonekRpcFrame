using WeDonekRpc.Client.Attr;
using WeDonekRpc.HttpApiGateway.Model;
namespace WeDonekRpc.HttpApiGateway.Interface
{
    [IgnoreIoc]
    public interface IApiServiceEvent : System.IDisposable
    {
        /// <summary>
        /// 初始化请求
        /// </summary>
        /// <param name="service"></param>
        void InitRequest (IApiService service);
        /// <summary>
        /// 初始化身份标识
        /// </summary>
        /// <param name="service"></param>
        void InitIdentity (IApiService service);
        /// <summary>
        /// 检查客户端缓存
        /// </summary>
        /// <param name="etag"></param>
        /// <param name="toUpdateTime"></param>
        /// <returns></returns>
        bool CheckCache (IApiService service, string etag, string toUpdateTime);
        /// <summary>
        /// 检查授权
        /// </summary>
        /// <param name="service"></param>
        /// <param name="source"></param>
        void CheckAccredit (IApiService service, ApiAccreditSet source);
        /// <summary>
        /// 响应事件
        /// </summary>
        /// <param name="service"></param>
        /// <param name="response"></param>
        void ReplyEvent (IApiService service, IResponse response);

        /// <summary>
        /// 结束请求
        /// </summary>
        /// <param name="service"></param>
        void EndRequest (IApiService service);
    }
}
