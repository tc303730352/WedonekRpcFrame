namespace HttpApiGateway.Interface
{
        /// <summary>
        /// 响应模板
        /// </summary>
        public interface IApiResponseTemplate
        {
                /// <summary>
                /// 获取错误响应
                /// </summary>
                /// <param name="error"></param>
                /// <returns></returns>
                IResponse GetErrorResponse(string error);
                /// <summary>
                /// 获取成功响应
                /// </summary>
                /// <returns></returns>
                IResponse GetResponse();
                /// <summary>
                /// 获取分页响应
                /// </summary>
                /// <param name="result"></param>
                /// <param name="count"></param>
                /// <returns></returns>
                IResponse GetResponse(object result, object count);
                /// <summary>
                /// 获取成功响应
                /// </summary>
                /// <param name="data"></param>
                /// <returns></returns>
                IResponse GetResponse(object result);
        }
}