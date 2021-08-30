namespace HttpService.Interface
{
        public interface IHttpHandler
        {
                /// <summary>
                /// 请求
                /// </summary>
                IHttpRequest Request
                {
                        get;
                }

                /// <summary>
                /// 响应
                /// </summary>
                IHttpResponse Response
                {
                        get;
                }
        }
}
