namespace ApiGateway.Interface
{
        /// <summary>
        /// 网关模块
        /// </summary>
        public interface IModular : System.IDisposable
        {
                /// <summary>
                /// 模块服务名
                /// </summary>
                string ServiceName { get; }
                /// <summary>
                /// Api路由格式
                /// </summary>
                string ApiRouteFormat { get; }

                /// <summary>
                /// 初始化模块
                /// </summary>
                void InitModular();

                void Start();
        }
}
