using System.IO;
using WeDonekRpc.Helper;

namespace WeDonekRpc.WebSocketGateway.Interface
{
    /// <summary>
    /// 响应模板
    /// </summary>
    public interface IResponseTemplate
    {
        /// <summary>
        /// 获取错误响应
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        string GetErrorResponse(IUserPage page, string error);
        /// <summary>
        /// 获取错误响应
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        string GetErrorResponse(string error);
        /// <summary>
        /// 获取错误响应
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        string GetErrorResponse(IUserPage page, ErrorException error);

        /// <summary>
        /// 获取成功响应
        /// </summary>
        /// <returns></returns>
        string GetResponse(IUserPage page);
        /// <summary>
        /// 授权成功
        /// </summary>
        /// <returns></returns>
        string AuthorizationSuccess();

        /// <summary>
        /// 获取成功响应
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        string GetResponse(IUserPage page, object result);

        /// <summary>
        /// 授权失败
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        string AuthorizationFail(ErrorException error);

        /// <summary>
        /// 获取全局错误响应
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        string GetErrorResponse(ErrorException error);
    }
}
